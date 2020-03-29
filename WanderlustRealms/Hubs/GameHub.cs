using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Interfaces;
using WanderlustRealms.Models.Enum;
using WanderlustRealms.Models.Items;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Races;
using WanderlustRealms.Models.Rooms;
using WanderlustRealms.Services;

namespace WanderlustRealms.Hubs
{
    public class GameHub : Hub
    {
        private ApplicationDbContext _context;
        private static List<Room> _roomRepository = new List<Room>();
        private static List<PlayerCharacter> _playerRepository = new List<PlayerCharacter>();
        private static List<Item> _itemRepository = new List<Item>();
        private static List<NPC> _npcRepository = new List<NPC>();
        private static List<Language> _languageRepository = new List<Language>();
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public GameHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public List<string> GetAllConnectedUsers()
        {
            return _playerRepository.Select(X => X.Name).ToList();
        }

        public async Task SendPlayerAction(Room currentRoom, Living living, string action, string target)
        {
            var subject = "";
            var verb = "";
            var obj = "";

            foreach(PlayerCharacter p in currentRoom.PCs)
            {
                foreach(var connectionId in _connections.GetConnections(p.Name))
                {
                    subject = living.Name;

                    if(p.Name == living.Name)
                    {
                        subject = "You";
                        verb = action;
                        obj = target;
                    }
                    else
                    {
                        verb = action + "s";
                        obj = target;

                        if(target == p.Name.ToLower())
                        {
                            obj = "you";
                        }

                    }

                    await Clients.Client(connectionId).SendAsync("ReceiveActionMessage", subject, verb, obj);
                }
            }
        }

        public async Task SendErrorMessage(PlayerCharacter pc, string msg)
        {
            var httpContext = Context.GetHttpContext();
            var query = httpContext.Request.Query;
            var playerName = query.GetQueryParameterValue<string>("PlayerName");

            foreach (var connectionId in _connections.GetConnections(playerName))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveErrorMessage", msg);
            }
        }

        public async Task SendRoomDescription(PlayerCharacter pc, Room room)
        {
            var itemList = CreateItemGrouping(room.ItemList);
            string itemString = "";

            if(itemList.Count() > 2)
            {
                itemString = String.Join(", ", itemList.ToArray(), 0, itemList.Count - 1) + ", and " + itemList.LastOrDefault();
            }
            else if(itemList.Count == 2)
            {
                itemString = String.Join(", ", itemList.ToArray(), 0, itemList.Count - 1) + " and " + itemList.LastOrDefault();
            }
            else if(itemList.Count == 1)
            {
                itemString = itemList[0];
            }
            

            var npcList = CreateNPCGrouping(room.NPCList);
            string npcString = "";

            if (npcList.Count() > 2)
            {
                npcString = String.Join(", ", npcList.ToArray(), 0, npcList.Count - 1) + ", and " + npcList.LastOrDefault();
            }else if(npcList.Count == 2)
            {
                npcString = String.Join(", ", npcList.ToArray(), 0, npcList.Count - 1) + " and " + npcList.LastOrDefault();
            }
            else if(npcList.Count == 1)
            {
                npcString = npcList[0];
            }

            var playerList = ListNouns(room.PCs.Where(x => x.LivingID != pc.LivingID).OrderBy(x => x.Name).Select(x => x.Name).ToList());
            
            foreach (var connectionId in _connections.GetConnections(pc.Name))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveRoomMessage", room.Title, room.Description, room.RoomExits, itemString, npcString, playerList);
            }
        }

        public async Task JoinRoom(int roomID)
        {
            var httpContext = Context.GetHttpContext();
            var query = httpContext.Request.Query;
            var LivingID = query.GetQueryParameterValue<int>("LivingID");
            var player = _playerRepository.Where(x => x.LivingID == LivingID).FirstOrDefault();

            var targetRoom = LoadRoom(roomID);

            if (!targetRoom.PCs.Contains(player))
            {
                targetRoom.PCs.Add(player);
            }

            await SendRoomDescription(player, targetRoom);
        }

        public async Task LeaveRoom(int roomID)
        {
            var httpContext = Context.GetHttpContext();
            var query = httpContext.Request.Query;
            var LivingID = query.GetQueryParameterValue<int>("LivingID");

            //Grab room from repository or db
            var targetRoom = LoadRoom(roomID);
            targetRoom.PCs.Remove(_playerRepository.Where(x => x.LivingID == LivingID).FirstOrDefault());

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomID.ToString());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var query = httpContext.Request.Query;
            var LivingID = query.GetQueryParameterValue<int>("LivingID");

            var player = _playerRepository.Where(x => x.LivingID == LivingID).FirstOrDefault();
            await SavePlayer(player);
            RemovePlayeFromRoom(LivingID);
            
            _playerRepository.Remove(player);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var query = httpContext.Request.Query;
            var LivingID = query.GetQueryParameterValue<int>("LivingID");
            var player = new PlayerCharacter();

            if (_playerRepository.Any(x => x.LivingID == LivingID))
            {
                player = _playerRepository.Where(x => x.LivingID == LivingID).First();
            }
            else
            {
                player = _context.PlayerCharacters.Where(x => x.LivingID == LivingID)
                .Include(x => x.Race)
                .ThenInclude(x => x.Body)
                .Include(x => x.Background)
                .Include(x => x.RoomKingdom)
                .Include(x => x.PlayerSkills).ThenInclude(y => y.Skill)
                .FirstOrDefault();

                player.Race.Body.Limbs = _context.BodyLimbJoins.Where(x => x.BodyID == player.Race.Body.BodyID).Select(X => X.Limb).ToList();
                player.Actions = _context.GameActions.Where(x => x.IsStandard).ToList();
                player.Actions.AddRange(_context.GameActionJoins.Where(x => x.LivingID == player.LivingID).Select(X => X.GameAction).ToList());

                //Load Languages
                player.Languages = _context.LivingLanguages.Where(x => x.LivingID == player.LivingID).Include(x => x.Language).ToList();

                //Load any wielded/worn weapons and armor
                var items = _context.LimbItems.Where(x => x.LivingID == player.LivingID).ToList();

                foreach(var l in player.Race.Body.Limbs)
                {
                    var currentItem = items.Where(x => x.LimbID == l.LimbID).FirstOrDefault();

                    if(currentItem != null && currentItem.Item != null)
                    {
                        if (l.EquippedItem == null)
                        {
                            l.EquippedItem = currentItem.Item;
                        }
                    }
                }

                //Get rid of the limb items
                _context.RemoveRange(items);
                await _context.SaveChangesAsync();

                LoadPlayerInventory(player);

                _playerRepository.Add(player);
            }

            _connections.Add(player.Name.ToString(), Context.ConnectionId);

            var loadRoom = LoadRoom(player.LastRoomID);

            if(loadRoom != null)
            {
                await JoinRoom(loadRoom.RoomID);
            }
            else
            {
                await JoinRoom(1);
            }

        }

        public async Task SavePlayer(PlayerCharacter pc)
        {
            pc.LastRoomID = GetPlayerCurrentRoom().RoomID;

            //Save their inventory
            pc.ItemIDList = string.Join(", ", pc.InventoryList.Select(X => X.ItemID).ToList());

            foreach(var l in pc.Race.Body.Limbs.Where(x => x.EquippedItem != null).ToList())
            {
                LimbItem li = new LimbItem();
                li.ItemID = l.EquippedItem.ItemID;
                li.LivingID = pc.LivingID;
                li.LimbID = l.LimbID;
                _context.Add(li);
            } 

            _context.Update(pc);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                var y = 5;
            }
            
        }

        public async Task ParseInput(string user, string input)
        {
            var httpContext = Context.GetHttpContext();
            var query = httpContext.Request.Query;
            var LivingID = query.GetQueryParameterValue<int>("LivingID");
            var player = _playerRepository.Where(x => x.LivingID == LivingID).FirstOrDefault();

            string[] parsedInput = input.ToLower().Split(" ");
            if(parsedInput.Count() > 0)
            {
                var verb = parsedInput[0];
                var predicate = parsedInput.Skip(1).ToList();

                //Is this action available to the player?
                var action = player.Actions.Where(x => x.Verb.ToLower() == verb).FirstOrDefault();
                if(action == null)
                {
                   action = player.Actions.Where(x => !string.IsNullOrEmpty(x.AlternateKeywords) && x.AlternateKeywords.ToLower() == verb).FirstOrDefault();
                }

                if(action != null)
                {
                    Type thisType = this.GetType();
                    MethodInfo theMethod = thisType.GetMethod(action.Function);

                    if(theMethod == null)
                    {
                        await SendErrorMessage(player, "Command not understood.");
                       
                    }
                    else
                    {
                        //Special exception
                        if (action.Function == "Move")
                        {
                            Task result = (Task)theMethod.Invoke(this, new object[] { player, GetPlayerCurrentRoom(), DirectionNormalizer(verb) });
                            await result;
                        }
                        else
                        {
                            var paramLength = theMethod.GetParameters().Count();

                            object[] objList = new object[paramLength];
                            var counter = 0;

                            foreach (ParameterInfo p in theMethod.GetParameters())
                            {

                                if (p.Name == "pc")
                                {
                                    try
                                    {
                                        objList[counter] = player;
                                    }
                                    catch (Exception e)
                                    {
                                        var y = 5;
                                    }

                                }
                                else if (p.Name == "room")
                                {
                                    objList[counter] = GetPlayerCurrentRoom();
                                }
                                else if (p.Name == "predicate")
                                {
                                    objList[counter] = predicate;
                                }

                                counter += 1;
                            }

                            Task result = (Task)theMethod.Invoke(this, objList);
                            await result;
                        }
                    }
                   
                }
                else
                {
                    await SendErrorMessage(player, "Command not understood.");
                }
            }
        }

        #region Actions

        public async Task Languages(PlayerCharacter pc)
        {
            var langList = _context.LivingLanguages.Where(x => x.LivingID == pc.LivingID).Include(x => x.Language).OrderBy(x => x.Language.Name).ToList();
            LanguageService service = new LanguageService();
            var mReturn = new List<string>();

            var mainLang = _context.Languages.Where(x => x.LanguageID == pc.MainLanguageID).FirstOrDefault();

            foreach(var lang in langList.Where(x => x.LanguageID != mainLang.LanguageID).ToList())
            {
                mReturn.Add(lang.Language.Name + " (" + service.GetFluency(lang.Level) + ")");
            }

            foreach (var connectionId in _connections.GetConnections(pc.Name))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", "Your main language is " + mainLang.Name + ". You also know " + ListNouns(mReturn) + ".");
            }
        }

        public async Task Say(PlayerCharacter pc, Room room, List<string> predicate)
        {
            var currentLanguage = new Language();
            var targetLang = pc.Languages.Where(x => x.Language.LanguageID == pc.MainLanguageID).Select(x => x.Language).FirstOrDefault();
            var messageString = "";
            LanguageService service = new LanguageService();

            if (predicate[predicate.Count - 1] == "in")
            {
                messageString = string.Join(" ", predicate.Take(predicate.Count - 2));
                targetLang = pc.Languages.Where(x => x.Language.Name.ToLower() == predicate[predicate.Count].ToLower()).Select(x => x.Language).FirstOrDefault();
            }
            else
            {
                messageString = string.Join(" ", predicate);
            }

            if(targetLang == null)
            {
                await SendErrorMessage(pc, "You don't know that language.");
            }
            else
            {
                //Get the room so we can get the players
                var currentRoom = GetPlayerCurrentRoom();

                SendLanguageMessage(pc, currentRoom, targetLang, messageString);                
            }

        }

        public async Task TrainSkill(PlayerCharacter pc, Room room, List<string> predicate)
        {
            TrainerService service = new TrainerService(_context);
            LanguageService lService = new LanguageService();

            var trainer = room.NPCList.Where(x => x.IsTrainer).FirstOrDefault();
            var skills = service.GetTrainerSkills(trainer.LivingID);

            if (trainer != null)
            {
                if (predicate[0].ToLower() == "list")
                {
                    //Ask the trainer to list his skills.
                    string itemString = "";

                    if (skills.Count() > 2)
                    {
                        itemString = String.Join(", ", skills.ToArray(), 0, skills.Count - 1) + ", and " + skills.LastOrDefault();
                    }
                    else if (skills.Count == 2)
                    {
                        itemString = String.Join(", ", skills.ToArray(), 0, skills.Count - 1) + " and " + skills.LastOrDefault();
                    }
                    else if (skills.Count == 1)
                    {
                        itemString = skills[0];
                    }

                    var msg = pc.Name + ", I can give instruction in " + itemString;
                    var targetLang = trainer.Languages.Where(x => x.LanguageID == trainer.MainLanguageID).FirstOrDefault();
                    
                    //If they don't have a native tongue default to Kingdom Common
                    if(targetLang == null)
                    {
                        var kc = LoadLanguageByName("Kingdom Common");
                        targetLang = new LivingLanguage() { LanguageID = kc.LanguageID, LivingID = trainer.LivingID, Language = kc }; 
                    }

                    SendLanguageMessage(trainer, GetPlayerCurrentRoom(), targetLang.Language, msg);
                }



            }
            else
            {
                await SendErrorMessage(pc, "There is no trainer here.");
            }
        }

        public async Task Take(PlayerCharacter pc, Room room, List<string> predicate)
        {
            var target = "";
            var number = predicate.ElementAtOrDefault(predicate.Count - 1);
            var outNumber = 0;

            //if number is a number, then the preceding parts are targets
            if (Int32.TryParse(number, out outNumber))
            {
                target = string.Join(" ", predicate.Take(predicate.Count - 1).ToList());
            }
            else
            {
                target = string.Join(" ", predicate);
            }

            string errorMsg = "";

            if (TargetExistsInRoom(room, target))
            {
                //Get the item
                var item = room.ItemList.Where(x => x.Name.ToLower() == target.ToLower() || (!string.IsNullOrEmpty(x.Keywords) && x.Keywords.ToLower().Contains(target.ToLower()))).FirstOrDefault();

                if (item != null)
                {
                    //Can you even take this?
                    if (item.CanTake)
                    {
                        //Check encumbrance
                        if (EncumbranceCheck(pc, item))
                        {
                            RemoveItemFromRoom(room, item);
                            AddItemToPlayerInventory(item, pc);
                            await SendPlayerAction(room, pc, "take", item.Name);
                        }
                        else
                        {
                            errorMsg = "That is too heavy for you to carry.";
                            await SendErrorMessage(pc, errorMsg);
                        }
                    }
                    else
                    {
                        await SendErrorMessage(pc, "You cannot take that.");
                    }
                    
                }
                else
                {
                    await SendErrorMessage(pc, "Take what?");
                }
            }
            else
            {
                errorMsg = "Take what?";
                await SendErrorMessage(pc, errorMsg);
            }
        }

        public async Task Drop(PlayerCharacter pc, Room room, List<string> predicate)
        {
            var target = predicate[0];

            string errorMsg;

            if (TargetExistsInInventory(pc, target))
            {
                var item = pc.InventoryList.Where(x => x.Name.ToLower() == target || x.Keywords.ToLower().Contains(target)).FirstOrDefault();

                if (item != null)
                {
                    AddItemToRoom(room, item);
                    RemoveItemFromPlayerInventory(item, pc);
                    await SendPlayerAction(room, pc, "drop", item.Name);
                }
            }
            else
            {
                errorMsg = "Drop what?";
                await SendErrorMessage(pc, errorMsg);
            }

        }

        public async Task Look(PlayerCharacter pc, Room room, List<string> predicate)
        {
            var target = "";
            var number = predicate.ElementAtOrDefault(predicate.Count - 1);
            var outNumber = 0;

            //if number is a number, then the preceding parts are targets
            if(Int32.TryParse(number, out outNumber))
            {
                target = string.Join(" ", predicate.Take(predicate.Count - 1).ToList());
            }
            else
            {
                target = string.Join(" ", predicate);
            }
            
            if(string.IsNullOrEmpty(target))
            {
                await SendRoomDescription(pc, room);
            }
            else
            {
                var lookupDict = CreateRoomDictionary(room);

                if (lookupDict.ContainsKey(target.ToLower()))
                {
                    var describer = lookupDict[target.ToLower()].ElementAtOrDefault(outNumber);

                    if(describer != null)
                    {
                        foreach (var connectionId in _connections.GetConnections(pc.Name))
                        {
                            await Clients.Client(connectionId).SendAsync("ReceiveLookMessage", describer.GetShortDescription(), describer.GetLongDescription());
                        }
                    }
                    else
                    {
                        await SendErrorMessage(pc, "Look at what?");
                    }
                }
                else
                {
                    await SendErrorMessage(pc, "Look at what?");
                }
            }
        }

        public async Task Skills(PlayerCharacter pc)
        {
            foreach (var connectionId in _connections.GetConnections(pc.Name))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveSkillsMessage", pc.PlayerSkills.Where(x => x.Level > 0).OrderBy(x => x.Skill.Name).ToList());
            }
        }

        public async Task Inventory(PlayerCharacter pc)
        {
            //Need to mark items as worn
            var equipList = new List<Item>();
            
            foreach(var l in pc.Race.Body.Limbs)
            {
                if(l.EquippedItem != null)
                {
                    equipList.Add(l.EquippedItem);
                }
            }

            equipList = equipList.Distinct().ToList();

            var equipedItemList = string.Join(" ", WornEquipped(equipList));
            var itemList = CreateItemGrouping(pc.InventoryList);
            var itemString = "";

            if(itemList.Count() == 1)
            {
                itemString = "You are carrying: " + itemList[0];
            }else if(itemList.Count() == 2)
            {
                itemString = "You are carrying: " + String.Join(", ", itemList.ToArray(), 0, itemList.Count - 1) + " and " + itemList.LastOrDefault();
            }
            else if(itemList.Count > 2)
            {
                itemString = "You are carrying: " + String.Join(", ", itemList.ToArray(), 0, itemList.Count - 1) + ", and " + itemList.LastOrDefault();
            }
            else
            {
                itemString = "You are not carrying anything";
            }

            foreach (var connectionId in _connections.GetConnections(pc.Name))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveInventoryMessage", itemString, equipedItemList, GetPlayerEncumbrance(pc), GetPlayerMaxEncumbrance(pc));
            }
        }

        public async Task Limbs(PlayerCharacter pc)
        {
            foreach (var connectionId in _connections.GetConnections(pc.Name))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveLimbsMessage", pc.Race.Body.Limbs.OrderBy(X => X.SortOrder).ToList());
            }
        }

        public async Task CheckGold(PlayerCharacter pc)
        {
            foreach (var connectionId in _connections.GetConnections(pc.Name))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveGoldMessage", pc.Gold);
            }
        }

        public async Task Vitals(PlayerCharacter pc)
        {
            foreach (var connectionId in _connections.GetConnections(pc.Name))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveVitals", pc.MaxHP, pc.CurrentHP, pc.MaxMP, pc.CurrentMP);
            }
        }

        public async Task Move(PlayerCharacter pc, Room room, string target)
        {
            target = DirectionNormalizer(target.ToLower());

            if(CheckRoomExit(room.RoomID, target))
            {
                var targetRoom = LoadRoom(room.RoomExits.Where(x => x.ExitDesc.ToLower() == target).Select(x => x.TargetRoomID).FirstOrDefault());

                await LeaveRoom(room.RoomID);
                await SendPlayerAction(room, pc, "leave", "the room heading " + target);

                await JoinRoom(targetRoom.RoomID);
            }
            else
            {
                await SendErrorMessage(pc, "You cannot go that direction.");
            }
        }

        public async Task Wear(PlayerCharacter pc, List<string> predicate)
        {
            var target = string.Join(" ", predicate);
            WearService service = new WearService();

            if (TargetExistsInInventory(pc, target))
            {
                var item = pc.InventoryList.Where(x => x.Name.ToLower() == target).FirstOrDefault();

                if(item == null)
                {
                    item = pc.InventoryList.Where(x => x.Keywords.ToLower().Contains(target)).FirstOrDefault();
                }

                if(item != null)
                {
                    //Only armor is wearable
                    if(item.ItemType == ItemTypes.Armor)
                    {
                        //Load amror limbs
                        var armor = (Armor)item;

                        //Can the race wear it?
                        if (!service.RaceCanWear(pc, armor)){

                            await SendErrorMessage(pc, "As a " + pc.Race.Name + " you cannot wear that.");
                            return;
                        }

                        var limbIDList = armor.LimbIDList.Split(",");

                        foreach(var s in limbIDList)
                        {
                            Limb l = _context.Limbs.Find(Int32.Parse(s));
                            armor.Limbs.Add(l);
                        }

                        var limbOccupied = 0;
                        var alreadyWornMsg = new List<string>();

                        //Make sure all the limbs aren't currently occupied
                        foreach(var limb in armor.Limbs)
                        {
                            var currentLimb = pc.Race.Body.Limbs.Where(x => x.LimbID == limb.LimbID).FirstOrDefault();

                            if(currentLimb.EquippedItem != null)
                            {
                                limbOccupied += 1;
                                alreadyWornMsg.Add(currentLimb.EquippedItem.Name);
                            }
                        }

                        if(limbOccupied == 0)
                        {
                            foreach(var limb in armor.Limbs)
                            {
                                var currentLimb = pc.Race.Body.Limbs.Where(x => x.LimbID == limb.LimbID).FirstOrDefault();
                                currentLimb.EquippedItem = item;
                            }

                            pc.InventoryList.Remove(item);

                            await SendPlayerAction(GetPlayerCurrentRoom(), pc, "wear", IndefiniteArticle(item.Name) + " " + item.Name);
                        }
                        else
                        {
                            alreadyWornMsg = alreadyWornMsg.Distinct().ToList();

                            await SendErrorMessage(pc, "You are already wearing the " + alreadyWornMsg[0]);
                        }
                    }
                    else
                    {
                        await SendErrorMessage(pc, "You cannot wear that.");
                    }
                }
            }
            else
            {
                await SendErrorMessage(pc, "That is not in your inventory.");
            }
        }

        public async Task Wield(PlayerCharacter pc, List<string> predicate)
        {
            var target = string.Join(" ", predicate);
            var limbTarget = "";
            WieldService service = new WieldService();
            SkillService skillService = new SkillService();

            if (predicate.Contains("in"))
            {
                var parseString = target.Split("in");
                if(parseString.Count() == 2)
                {
                    target = parseString[0].Trim();
                    limbTarget = parseString[1].Trim();
                }
            }

            if (TargetExistsInInventory(pc, target))
            {
                var item = GetItemFromPlayerInventory(pc, target);
                
                if(item.ItemType != ItemTypes.Weapon)
                {
                    await SendErrorMessage(pc, "That is not a weapon.");
                    return;
                }

                var weapon = new Weapon();
                try
                {
                    weapon = (Weapon)item;
                }
                catch(Exception ex)
                {
                    await SendErrorMessage(pc, "That is not a weapon.");
                    return;
                }


                if (!service.CheckRaceWield(pc, weapon))
                {
                    await SendErrorMessage(pc, "As a " + pc.Race.Name + " you cannot wield that.");
                    return;
                }

                if (!service.LimbAvailable(pc))
                {
                    await SendErrorMessage(pc, "Your hands are full.");
                    return;
                }

                if(!service.CheckHandednessAvailability(pc, weapon))
                {
                    await SendErrorMessage(pc, "You do not have an available hand to do that.");
                    return;
                }

                var limbToTarget = new List<Limb>();

                if (!string.IsNullOrEmpty(limbTarget))
                {
                    if(!pc.Race.Body.Limbs.Any(x => x.Name.ToLower() == limbTarget.ToLower()))
                    {
                        await SendErrorMessage(pc, "You do not have " + IndefiniteArticle(limbTarget));
                        return;
                    }

                    if(service.LimbIsFull(pc, limbTarget))
                    {
                        await SendErrorMessage(pc, "You are already wielding a weapon in your " + limbTarget);
                        return;
                    }

                    limbToTarget.Add(pc.Race.Body.Limbs.Where(x => x.Name.ToLower() == limbTarget.ToLower()).FirstOrDefault());
                }
                else
                {
                    limbToTarget.AddRange(service.GetAvailableLimbs(pc, weapon));
                }

                var maxItem = (pc.Race.Body.Limbs.Max(x => x.ItemTag)) + 10;

                //Now let's assign the weapon to the limbs
                foreach (var l in limbToTarget)
                {
                    var currentLimb = pc.Race.Body.Limbs.Where(X => X.LimbID == l.LimbID).FirstOrDefault();
                    currentLimb.EquippedItem = weapon;
                    currentLimb.ItemTag = maxItem;                    
                }

                pc.InventoryList.Remove(item);

                if (!string.IsNullOrEmpty(limbTarget))
                {
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "wield", IndefiniteArticle(item.Name) + " " + item.Name + "in your " + limbTarget.ToLower());
                }
                else
                {
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "wield", IndefiniteArticle(item.Name) + " " + item.Name);
                }
                

                if (service.IsPlayerDualWielding(pc))
                {
                    await SendErrorMessage(pc, skillService.TwoHandedFightingMessage(pc));
                }
            }
            else
            {
                await SendErrorMessage(pc, "You don't have that.");
            }
        }

        public async Task Remove(PlayerCharacter pc, List<string> predicate)
        {
            var target = string.Join(" ", predicate);

            if (PlayerIsWearingItem(pc, target))
            {
                var item = new Item();

                foreach(var limb in pc.Race.Body.Limbs)
                {
                    if(limb.EquippedItem != null)
                    {
                        if(limb.EquippedItem.Name.ToLower() == target || limb.EquippedItem.Keywords.Contains(target))
                        {
                            item = limb.EquippedItem;
                            limb.EquippedItem = null;
                            break;
                        }
                    }
                }

                if(item.ItemID != 0)
                {
                    pc.InventoryList.Add(item);
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "remove", IndefiniteArticle(item.Name) + " " + item.Name);
                }
                else
                {
                    await SendErrorMessage(pc, "You are not wearing that.");
                }
            }
            else
            {
                await SendErrorMessage(pc, "You are not wearing that.");
            }
        }

        public async Task Unwield(PlayerCharacter pc, List<string> predicate)
        {
            var target = string.Join(" ", predicate);
            var limbTarget = "";
            WieldService service = new WieldService();
            SkillService skillService = new SkillService();

            if (predicate.Contains("in"))
            {
                var parseString = target.Split("in");
                if (parseString.Count() == 2)
                {
                    target = parseString[0].Trim();
                    limbTarget = parseString[1].Trim();
                }
            }

            if (PlayerIsWearingItem(pc, target))
            {
                var item = new Item();

                if (!string.IsNullOrEmpty(limbTarget))
                {
                    var limb = pc.Race.Body.Limbs.Where(x => x.Name.ToLower() == limbTarget.ToLower()).FirstOrDefault();
                    item = limb.EquippedItem;
                    limb.EquippedItem = null;

                    pc.InventoryList.Add(item);
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "unwield", IndefiniteArticle(item.Name) + " " + item.Name);
                }
                else
                {
                    var itemLimb = pc.Race.Body.Limbs.Where(x => x.EquippedItem != null).Where(x => x.EquippedItem.Name.ToLower() == target.ToLower() || x.EquippedItem.Keywords.Contains(target.ToLower())).Select(x => new { x.EquippedItem, x.ItemTag }).FirstOrDefault();
                    var limbsToUnwield = pc.Race.Body.Limbs.Where(x => x.EquippedItem !=null && x.EquippedItem.ItemID == itemLimb.EquippedItem.ItemID && x.ItemTag == itemLimb.ItemTag).ToList();

                    foreach(var l in limbsToUnwield)
                    {
                        l.EquippedItem = null;
                    }

                    item = LoadItem(itemLimb.EquippedItem.ItemID);

                    pc.InventoryList.Add(item);
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "unwield", IndefiniteArticle(item.Name) + " " + item.Name);
                }
            }
            else
            {
                await SendErrorMessage(pc, "You are not wielding that.");
            }
        }

        public async Task ListStoreItems(PlayerCharacter pc, Room room)
        {
            var shopKeep = room.NPCList.Where(x => x.IsShopKeep).FirstOrDefault();
            var shopName = _context.Shops.Where(x => x.LivingID == shopKeep.LivingID).Select(x => x.ShopName).FirstOrDefault();
            ShopService service = new ShopService(_context);

            if(shopKeep != null)
            {
                var shopItems = service.ListShopItemsByNPC(shopKeep.LivingID, pc);

                foreach (var connectionId in _connections.GetConnections(pc.Name))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveShopList", shopItems, shopName);
                }
            }
            else
            {
                await SendErrorMessage(pc, "You don't see a shopkeeper here.");
            }
        }

        public async Task Haggle(PlayerCharacter pc, List<string> predicate)
        {
            if(predicate.ElementAtOrDefault(0).ToLower() == "on")
            {
                pc.IsHaggleOn = true;
                await SendErrorMessage(pc, "You turn haggling on.");
            }

            if (predicate.ElementAtOrDefault(0).ToLower() == "off")
            {
                pc.IsHaggleOn = false;
                await SendErrorMessage(pc, "You turn haggling off.");
            }
        }

        public async Task SellItem(PlayerCharacter pc, Room room, List<string> predicate)
        {
            var shopKeep = room.NPCList.Where(x => x.IsShopKeep).FirstOrDefault();
            var target = string.Join(" ", predicate);
            var shop = _context.Shops.Where(x => x.LivingID == shopKeep.LivingID).Include(x => x.ShopItems).ThenInclude(y => y.Item).FirstOrDefault();
            ShopService service = new ShopService(_context);
            var shopItems = service.ListShopItemsByNPC(shopKeep.LivingID, pc);

            if (TargetExistsInInventory(pc, target))
            {
                var item = pc.InventoryList.Where(x => x.Name.ToLower() == target.ToLower()).FirstOrDefault();
                if(item == null)
                {
                    item = pc.InventoryList.Where(x => x.Keywords.ToLower().Contains(target.ToLower())).FirstOrDefault();
                }

                if (pc.IsHaggleOn)
                {
                    var haggleInfo = service.Haggle(pc, shop, item, false);

                    pc.Gold += haggleInfo.Item1;
                    pc.InventoryList.Remove(item);
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "haggle", " with " + shopKeep.Name);
                    await SendErrorMessage(pc, haggleInfo.Item2);
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "sell", IndefiniteArticle(item.Name) + " " + item.Name + " to " + shopKeep.Name + " for " + haggleInfo.Item1.ToString() + " gold");
                }
                else
                {
                    pc.Gold += item.Cost;
                    pc.InventoryList.Remove(item);
                    await SendPlayerAction(GetPlayerCurrentRoom(), pc, "sell", IndefiniteArticle(item.Name) + " " + item.Name + " to " + shopKeep.Name + " for " + item.Cost.ToString() + " gold");
                }
               
            }
            else
            {
                await SendErrorMessage(pc, "You do not have that in your inventory.");
            }
        }

        public async Task BuyItem(PlayerCharacter pc, Room room, List<string> predicate)
        {
            var shopKeep = room.NPCList.Where(x => x.IsShopKeep).FirstOrDefault();
            var target = string.Join(" ", predicate);
            var shop = _context.Shops.Where(x => x.LivingID == shopKeep.LivingID).Include(x => x.ShopItems).ThenInclude(y => y.Item).FirstOrDefault();
            ShopService service = new ShopService(_context);

            var shopItems = service.ListShopItemsByNPC(shopKeep.LivingID, pc);

            var item = shopItems.Where(x => x.Name.ToLower() == target).FirstOrDefault();

            if(item == null)
            {
                item = shopItems.Where(x => x.Keywords.ToLower().Contains(target)).FirstOrDefault();
            }

            if(item != null)
            {
                if(item.SalesCost == 0)
                {
                    item.SalesCost = item.Cost;
                }

                if(pc.Gold >= item.SalesCost)
                {
                    if (EncumbranceCheck(pc, item))
                    {
                        pc.Gold -= item.SalesCost;
                        pc.InventoryList.Add(item);

                        await SendPlayerAction(GetPlayerCurrentRoom(), pc, "buy", IndefiniteArticle(item.Name) + " " + item.Name + " from " + shopKeep.Name);
                    }
                    else
                    {
                        await SendErrorMessage(pc, shopKeep.Name + @"says: ""You cannot carry that.""");
                    }
                }
                else
                {
                    await SendErrorMessage(pc, shopKeep.Name + @"says: ""You do not have enough money.""");
                }
                
            }
            else
            {
                await SendErrorMessage(pc, target + " is not for sale here.");
            }
                
        }

        #endregion

        #region Encumbrance Functions

        private int GetWeaponWeightAfterSkill(PlayerCharacter p, Weapon w)
        {
            var weight = w.Weight;

            var wSkill = p.PlayerSkills.Where(x => x.Skill.Name == "Great Weapons").FirstOrDefault();

            if (wSkill != null)
            {
                weight = (weight / 2) + ((weight / 2) - (int)(wSkill.Level / 2));
            }

            if (weight < 1)
            {
                weight = 1;
            }

            return weight;
        }
        private int GetArmorWeightAfterSkill(PlayerCharacter p, Armor armor)
        {
            var weight = armor.Weight;

            switch (armor.ArmorClass)
            {
                case ArmorClass.Heavy:
                    var hSkill = p.PlayerSkills.Where(x => x.Skill.Name == "Heavy Armor").FirstOrDefault();

                    if (hSkill != null)
                    {
                        weight = (weight / 2) + ((weight / 2) - (int)(hSkill.Level / 2));
                    }

                    break;
                case ArmorClass.Light:
                    hSkill = p.PlayerSkills.Where(x => x.Skill.Name == "Light Armor").FirstOrDefault();

                    if (hSkill != null)
                    {
                        weight = (weight / 2) + ((weight / 2) - (int)(hSkill.Level / 2));
                    }
                    break;
                case ArmorClass.Medium:
                    hSkill = p.PlayerSkills.Where(x => x.Skill.Name == "M Armor").FirstOrDefault();

                    if (hSkill != null)
                    {
                        weight = (weight / 2) + ((weight / 2) - (int)(hSkill.Level / 2));
                    }
                    break;
            }

            if (weight < 1)
            {
                weight = 1;
            }

            return weight;
        }

        public bool EncumbranceCheck(PlayerCharacter p, Item i)
        {
            //Encumbrance is 3x your durability initially
            var enc = p.Durability * 3;
            var weight = i.Weight;
            var currentweight = p.InventoryList.Sum(x => x.Weight);

            currentweight += GetPlayerWornItemEncumbrance(p);

            if (i.ItemType == ItemTypes.Armor)
            {
                var armor = (Armor)i;

                weight = GetArmorWeightAfterSkill(p, armor);
            }

            if ((currentweight + weight) > enc)
            {
                return false;
            }

            return true;
        }

        public int GetPlayerWornItemEncumbrance(PlayerCharacter pc)
        {
            var items = pc.Race.Body.Limbs.Where(x => x.EquippedItem != null).ToList();
            var mReturn = new List<Item>();
            var intList = new List<int>();

            foreach(var l in items)
            {
                if(l.ItemTag == 0)
                {
                    mReturn.Add(l.EquippedItem);
                }
                else
                {
                    if (!intList.Contains(l.ItemTag))
                    {
                        mReturn.Add(l.EquippedItem);
                        intList.Add(l.ItemTag);
                    }
                }
            }
            
            return mReturn.Sum(x => x.Weight);
        }

        public int GetPlayerEncumbrance(PlayerCharacter p)
        {
            var weight = 0;

            foreach (var i in p.InventoryList)
            {
                if (i.ItemType == ItemTypes.Armor)
                {
                    var armor = (Armor)i;

                    weight += GetArmorWeightAfterSkill(p, armor);
                }
                else if (i.ItemType == ItemTypes.Weapon)
                {
                    var weapon = (Weapon)i;

                    weight += GetWeaponWeightAfterSkill(p, weapon);
                }
                else
                {
                    weight += i.Weight;
                }
            }

            weight += GetPlayerWornItemEncumbrance(p);

            return weight;
        }

        public int GetPlayerMaxEncumbrance(PlayerCharacter p)
        {
            var skill = p.PlayerSkills.Where(x => x.Skill.Name == "Packing").Select(X => X.Level).FirstOrDefault();

            return (p.Durability * 3) + (int)(skill / 2);
        }

        #endregion

        #region Take and Drop Functions

        public void RemoveItemFromRoom(Room room, Item i)
        {
            room.ItemList.Remove(i);
        }

        public void AddItemToPlayerInventory(Item i, PlayerCharacter pc)
        {
            pc.InventoryList.Add(i);
        }

        public void AddItemToRoom(Room room, Item i)
        {
            room.ItemList.Add(i);
        }

        public void RemoveItemFromPlayerInventory(Item i, PlayerCharacter pc)
        {
            pc.InventoryList.Remove(i);
        }

        public bool TargetExistsInInventory(PlayerCharacter pc, string target)
        {
            return pc.InventoryList.Any(x => x.Name.ToLower() == target || x.Keywords.ToLower().Contains(target.ToLower()));
        }

        public bool PlayerIsWearingItem(PlayerCharacter pc, string target)
        {
            foreach(var limb in pc.Race.Body.Limbs)
            {
                if(limb.EquippedItem != null)
                {
                    if(limb.EquippedItem.Name.ToLower() == target || limb.EquippedItem.Keywords.Contains(target))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TargetExistsInRoom(Room room, string target)
        {
            if (room.ItemList.Any(x => x.Name.ToLower().Contains(target.ToLower())) || room.ItemList.Any(x => x.Keywords.ToLower().Contains(target.ToLower())))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Helper Functions

        public async void SendLanguageMessage(Living subject, Room currentRoom, Language targetLang, string messageString)
        {
            LanguageService service = new LanguageService();

            foreach (var connectionId in _connections.GetConnections(subject.Name))
            {
                var langAddOn = ".";

                //Is this the mother tongue?
                if(targetLang.LanguageID != subject.MainLanguageID)
                {
                    langAddOn = " in" + targetLang.Name + ".";
                }
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", "You say: '" + messageString + "'" + langAddOn);
            }

            foreach (var player in currentRoom.PCs.Where(x => x.LivingID != subject.LivingID).ToList())
            {
                //Do they have any knowledge of the language?
                if (player.Languages.Any(x => x.Language.LanguageID == targetLang.LanguageID))
                {
                    messageString = service.TransformSpeech(messageString, player.Languages.Where(x => x.Language.LanguageID == targetLang.LanguageID).Select(x => x.Level).First());

                    //native language?
                    if (player.MainLanguageID != targetLang.LanguageID)
                    {
                        messageString += " in " + targetLang.Name;
                    }
                }
                else
                {
                    messageString = "'" + service.TransformSpeech(messageString, 0);
                    messageString += "' in ";
                    messageString += IndefiniteArticle(service.GetLanguageModifier(targetLang));
                    messageString += " " + service.GetLanguageModifier(targetLang).ToLower();
                    messageString += " language.";
                }

                foreach (var connectionId in _connections.GetConnections(player.Name))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", subject.Name + " says: " + messageString);
                }
            }
        }
        public Language LoadLanguageByID(int LanguageID)
        {
            if(!_languageRepository.Any(x => x.LanguageID == LanguageID))
            {
                return _languageRepository.Where(x => x.LanguageID == LanguageID).First();
            }
            else
            {
                _languageRepository.Add(_context.Languages.Find(LanguageID));
                return _languageRepository.Where(x => x.LanguageID == LanguageID).First();
            }
        }

        public Language LoadLanguageByName(string name)
        {
            if (!_languageRepository.Any(x => x.Name.ToLower() == name.ToLower()))
            {
                return _languageRepository.Where(x => x.Name.ToLower() == name.ToLower()).First();
            }
            else
            {
                var lan = _context.Languages.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();

                if(lan != null)
                {
                    _languageRepository.Add(lan);
                    return _languageRepository.Where(x => x.Name.ToLower() == name.ToLower()).First();
                }

                return null;
            }
        }

        public Item GetItemFromPlayerInventory(PlayerCharacter pc, string name)
        {
            var item = pc.InventoryList.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();

            if(item == null)
            {
                return(pc.InventoryList.Where(x => x.Keywords.ToLower().Contains(name.ToLower())).FirstOrDefault());
            }
            else
            {
                return item;
            }
        }

        public Dictionary<string, List<IDescription>> CreateRoomDictionary(Room room)
        {
            Dictionary<string, List<IDescription>> lookupDict = new Dictionary<string, List<IDescription>>();

            //Go through items in room
            foreach(var item in room.ItemList)
            {
                if (!lookupDict.ContainsKey(item.Name.Trim().ToLower()))
                {
                    lookupDict.Add(item.Name.Trim().ToLower(), new List<IDescription>() { item });
                }
                else
                {
                    lookupDict[item.Name.Trim().ToLower()].Add(item);
                }

                if (!string.IsNullOrEmpty(item.Keywords))
                {
                    var keywords = item.Keywords.Split(",");

                    foreach (var keyword in keywords)
                    {
                        if (!lookupDict.ContainsKey(keyword.Trim().ToLower()))
                        {
                            lookupDict.Add(keyword.Trim().ToLower(), new List<IDescription>() { item });
                        }
                        else
                        {
                            lookupDict[keyword.Trim().ToLower()].Add(item);
                        }
                    }
                }
            }

            foreach(var npc in room.NPCList)
            {
                if (!lookupDict.ContainsKey(npc.Name.Trim().ToLower()))
                {
                    lookupDict.Add(npc.Name.Trim().ToLower(), new List<IDescription>() { npc });
                }
                else
                {
                    lookupDict[npc.Name.Trim().ToLower()].Add(npc);
                }

                if (!string.IsNullOrEmpty(npc.Keywords))
                {
                    var keywords = npc.Keywords.Split(",");
                    foreach (var keyword in keywords)
                    {
                        if (!lookupDict.ContainsKey(keyword))
                        {
                            lookupDict.Add(keyword.Trim().ToLower(), new List<IDescription>() { npc });
                        }
                        else
                        {
                            lookupDict[keyword.Trim().ToLower()].Add(npc);
                        }
                    }
                        
                }

                if(npc.Race != null)
                {
                    if (!lookupDict.ContainsKey(npc.Race.Name.ToLower()))
                    {
                        lookupDict.Add(npc.Race.Name.ToLower(), new List<IDescription> { npc });
                    }
                    else
                    {
                        lookupDict[npc.Race.Name.ToLower()].Add(npc);
                    }
                }

            }

            foreach(var p in room.PCs)
            {
                if (!lookupDict.ContainsKey(p.Name.ToLower()))
                {
                    lookupDict.Add(p.Name.ToLower(), new List<IDescription>() { p } );
                }
                else
                {
                    lookupDict[p.Name.ToLower()].Add(p);
                }

                if(p.Race != null)
                {
                    if (!lookupDict.ContainsKey(p.Race.Name.ToLower()))
                    {
                        lookupDict.Add(p.Race.Name.ToLower(), new List<IDescription> { p });
                    }
                    else
                    {
                        lookupDict[p.Race.Name.ToLower()].Add(p);
                    }
                }
            }

            return lookupDict;
        }
        public void LoadPlayerInventory(PlayerCharacter pc)
        {
            if(pc.ItemIDList != null)
            {
                var itemList = pc.ItemIDList.Split(",");

                foreach(var i in itemList)
                {
                    var id = 0;
                    var success = Int32.TryParse(i, out id);

                    if (success)
                    {
                        pc.InventoryList.Add(LoadItem(id));
                    }
                   
                }
            }
        }

        public Room GetPlayerCurrentRoom()
        {
            var httpContext = Context.GetHttpContext();
            var query = httpContext.Request.Query;
            var LivingID = query.GetQueryParameterValue<int>("LivingID");

            return(_roomRepository.Where(x => x.PCs.Any(x => x.LivingID == LivingID)).FirstOrDefault());
        }

        public void RemovePlayeFromRoom(int LivingID)
        {
            //Remove the player form any room he was in
            var loadRoom = _roomRepository.Where(x => x.PCs.Any(x => x.LivingID == LivingID)).FirstOrDefault();
            loadRoom.PCs.Remove(_playerRepository.Where(X => X.LivingID == LivingID).FirstOrDefault());
        }

        public bool CheckRoomExit(int RoomID, string direction)
        {
            var room = LoadRoom(RoomID);

            return room.RoomExits.Any(x => x.ExitDesc.ToLower() == direction);
        }

        public string DirectionNormalizer(string term)
        {
            if(term.ToLower() == "n")
            {
                return "north";
            }

            if (term.ToLower() == "s")
            {
                return "south";
            }

            if (term.ToLower() == "e")
            {
                return "east";
            }

            if (term.ToLower() == "w")
            {
                return "west";
            }

            if (term.ToLower() == "u")
            {
                return "up";
            }

            if (term.ToLower() == "d")
            {
                return "down";
            }

            return term;
        }

        private NPC LoadNPC(int LivingID)
        {
            var n = new NPC();

            if(!_npcRepository.Any(x => x.LivingID == LivingID))
            {
                n = _context.NPCs.Where(X => X.LivingID == LivingID).Include(x => x.Race).ThenInclude(y => y.Body).FirstOrDefault();

                //Load languages
                n.MainLanguageID = _context.RaceLanguages.Where(x => x.RaceID == n.Race.RaceID && x.IsMain).Select(x => x.LanguageID).FirstOrDefault();
                n.Languages = _context.RaceLanguages.Where(x => x.RaceID == n.Race.RaceID).Include(x => x.Language).Select(x => new LivingLanguage { Language = x.Language, LanguageID = x.LanguageID, LivingID = n.LivingID }).ToList();
                _npcRepository.Add(n);
            }

            return _npcRepository.Where(x => x.LivingID == LivingID).FirstOrDefault();
        }

        private Room LoadRoomNPC(Room room)
        {
            if(room.NPCIDList != null)
            {
                var list = room.NPCIDList.Split(",");

                if (list.Count() > 0)
                {
                    foreach (var s in list)
                    {
                        var id = 0;
                        var success = Int32.TryParse(s, out id);

                        if (success)
                        {
                            var npc = LoadNPC(id);
                            room.NPCList.Add(npc);
                        }
                    }
                }
            }

            return room;
        }

        private Item LoadItem(int ItemID)
        {
            var i = new Item();
            if(!_itemRepository.Any(x => x.ItemID == ItemID))
            {
                i = _context.Items.Where(x => x.ItemID == ItemID).FirstOrDefault();
                _itemRepository.Add(i);
                return i;
            }

            return _itemRepository.Where(x => x.ItemID == ItemID).FirstOrDefault();
        }

        private Room LoadRoomItems(Room room)
        {
            var itemIDList = new string[] { };

            if(room.ItemIDList != null)
            {
                try
                {
                    itemIDList = room.ItemIDList.Split(",");
                }
                catch (Exception ex)
                {
                    var y = 5;
                }


                if (itemIDList.Count() > 0)
                {
                    foreach (var s in itemIDList)
                    {
                        int id = 0;
                        var success = Int32.TryParse(s, out id);

                        room.ItemList.Add(LoadItem(id));
                    }
                }
            }

            return room;
        }

        private Room LoadRoom(int RoomID)
        {
            var r = _roomRepository.Where(x => x.RoomID == RoomID).FirstOrDefault();

            if(r != null)
            {
                return r;
            }

            r = _context.Rooms.Find(RoomID);

            if(r != null)
            {
                r.RoomExits = _context.RoomExits.Where(x => x.CurrentRoomID == r.RoomID).ToList();
                r = LoadRoomNPC(r);
                r = LoadRoomItems(r);
                _roomRepository.Add(r);

                return r;
            }

            return r;
        }

        #endregion

        #region View Helper Functions
        public List<string> WornEquipped(List<Item> itemList)
        {
            var mReturn = new List<string>();

            foreach(var i in itemList)
            {
                if(i.ItemType == ItemTypes.Armor)
                {
                    mReturn.Add(UppercaseFirst(i.Name) + "(worn)");
                }

                if(i.ItemType == ItemTypes.Weapon)
                {
                    mReturn.Add(UppercaseFirst(i.Name) + "(wielded)");
                }
            }

            return mReturn;
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public List<string> CreateNPCGrouping(List<NPC> livingList)
        {
            var mReturn = new List<string>();
            var ctDict = new Dictionary<string, int>();

            foreach(var npc in livingList.Where(x => !x.IsUnique))
            {
                var count = livingList.Where(x => x.GetName() == npc.GetName()).Count();

                if (!ctDict.ContainsKey(npc.GetName()))
                {
                    ctDict.Add(npc.GetName(), count);
                }
            }

            foreach (var npc in livingList.Where(x => x.IsUnique))
            {
                mReturn.Add(npc.Name);
            }

            foreach (KeyValuePair<string, int> kvp in ctDict)
            {
                var adj = "";
                var noun = "";

                if (kvp.Value > 1)
                {
                    adj = GetQuantitativeAdjective(kvp.Value);
                    noun = Pluralize(kvp.Key);
                }
                else
                {
                    adj = IndefiniteArticle(kvp.Key);
                    noun = kvp.Key;
                }

                mReturn.Add(adj + " " + noun);
            }

            return mReturn;
        }

        public List<string> CreateItemGrouping(List<Item> objectList)
        {
            var mReturn = new List<string>();
            var ctDict = new Dictionary<string, int>();

            foreach(var item in objectList)
            {
                var count = objectList.Where(x => x.GetName() == item.GetName()).Count();

                if (!ctDict.ContainsKey(item.GetName()))
                {
                    ctDict.Add(item.GetName(), count);
                }
            }

            foreach(KeyValuePair<string, int> kvp in ctDict)
            {
                var adj = "";
                var noun = "";

                if(kvp.Value > 1)
                {
                    adj = GetQuantitativeAdjective(kvp.Value);
                    noun = Pluralize(kvp.Key);
                }
                else
                {
                    adj = IndefiniteArticle(kvp.Key);
                    noun = kvp.Key;
                }

                mReturn.Add(adj + " " + noun);
            }

            return mReturn;
        }

        public string Pluralize(string s)
        {
            var specialList = new List<string>() { "s", "ss", "sh", "ch", "x", "z" };

            if(s.Length > 2)
            {
                var result = Regex.Match(s, @"(.{2})\s*$");

                if(specialList.Contains(result.Value) || specialList.Contains(result.Value.Substring(result.Value.Count() - 1, 1)))
                {
                    return s + "es";
                }
            }

            return s + "s";
        }

        public string GetQuantitativeAdjective(int count)
        {
            string mReturn = "a";

            if(count == 2)
            {
                return "a pair of";
            }else if(count > 2 && count < 6){
                return "several";
            }else if(count > 5 && count < 11)
            {
                return "many";
            }else if(count > 10)
            {
                return "a big pile of";
            }

            return mReturn;
        }

        public string IndefiniteArticle(string noun)
        {
            var vowels = new List<string>() { "a", "e", "i", "o", "u" };

            if (vowels.Contains(noun[0].ToString().ToLower()))
            {
                return "an";
            }

            return "a";
        }

        public string ListNouns(List<string> nouns)
        {
            var itemString = "";

            if (nouns.Count() > 2)
            {
                itemString = String.Join(", ", nouns.ToArray(), 0, nouns.Count - 1) + ", and " + nouns.LastOrDefault();
            }
            else if (nouns.Count == 2)
            {
                itemString = String.Join(", ", nouns.ToArray(), 0, nouns.Count - 1) + " and " + nouns.LastOrDefault();
            }
            else if (nouns.Count == 1)
            {
                itemString = nouns[0];
            }

            return itemString;
        }

        #endregion
    }
}
