using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderlustRealms.Data;
using WanderlustRealms.Models;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Skills;
using WanderlustRealms.Models.ViewModel;
using WanderlustRealms.Services;

namespace WanderlustRealms.Controllers
{
    public class PlayerCharactersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayerCharactersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region AjaxFunctions

        [HttpGet]
        public IActionResult GetRaceBackgrounds(int RaceID)
        {
            return Json(_context.RaceBackgrounds.Where(x => x.RaceID == RaceID).Select(x => x.PlayerBackground).OrderBy(x => x.Name).ToList());
        }

        [HttpGet]
        public IActionResult GetRaceList()
        {
            return Json(_context.Races.OrderBy(x => x.Name).ToList());
        }

        [HttpGet]
        public IActionResult GetNameExists(string name)
        {
            return Ok(_context.Living.Any(x => x.Name.ToLower() == name.ToLower()));
        }

        public IActionResult SkillCalculator(int RaceID, int PlayerBackgroundID)
        {
            return ViewComponent("SkillCalculator", new { RaceID = RaceID, PlayerBackgroundID = PlayerBackgroundID });
        }

        public IActionResult StatCalculator(int RaceID)
        {
            return ViewComponent("StatCalculator", new { RaceID = RaceID });
        }

        #endregion

        public async Task<IActionResult> CharacterDetails(int id)
        {

            return ViewComponent("CharacterDetails", new { LivingID = id });
        }

        public async Task<IActionResult> NewCharacter()
        {
            ViewData["RaceID"] = new SelectList(_context.Races.OrderBy(x => x.Name), "RaceID", "Name");
            ViewData["KingdomList"] = new SelectList(_context.RoomKingdoms.OrderBy(x => x.Name), "RoomKingdomID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCharacter(PCViewModel vm)
        {
            var user = await _userManager.GetUserAsync(User);
            PlayerCharacterService service = new PlayerCharacterService(_context);

            PlayerCharacter p = new PlayerCharacter();

            p.Name = vm.Name;
            p.IsActive = true;
            p.Level = 1;
            p.RaceID = vm.RaceID;
            p.RoomKingdomID = vm.RoomKingdomID;
            p.UserID = user.Id;
            p.PlayerBackgroundID = vm.PlayerBackgroundID;
            p.XP = 0;
            p.CreatedDate = DateTime.UtcNow;
            p.CreatedBy = user.Id;
            p.Gender = vm.Gender;

            if(vm.GoodAlignChoice == Models.Enum.GoodAlignmentChoices.Evil)
            {
                p.GoodAlignment = 0;
            }else if(vm.GoodAlignChoice == Models.Enum.GoodAlignmentChoices.Neutral)
            {
                p.GoodAlignment = 50;
            }
            else
            {
                p.GoodAlignment = 100;
            }

            if (vm.OrderAlignChoice == Models.Enum.OrderAlignmentChoices.Chaotic)
            {
                p.OrderAlignment = 0;
            }
            else if (vm.OrderAlignChoice == Models.Enum.OrderAlignmentChoices.Neutral)
            {
                p.OrderAlignment = 50;
            }
            else
            {
                p.OrderAlignment = 100;
            }

            p.Durability = vm.SetDur;
            p.Dexterity = vm.SetDex;
            p.Intellect = vm.SetInt;
            p.Intuition = vm.SetIntuit;
            p.Charisma = vm.SetCha;
            p.Willpower = vm.SetWill;
            p.Description = "";
            p.ShortDesc = "";

            _context.PlayerCharacters.Add(p);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                var x = 5;
            }

            //Let's handle languages
            var languages = _context.RaceLanguages.Where(x => x.RaceID == p.RaceID).Include(x => x.Language).ToList();

            foreach(var l in languages)
            {
                LivingLanguage ll = new LivingLanguage();
                ll.LivingID = p.LivingID;
                ll.LanguageID = l.Language.LanguageID;
                ll.Level = l.StartingLevel;
               
                _context.Add(ll);
            }

            p.MainLanguageID = languages.Where(x => x.IsMain).Select(x => x.LanguageID).FirstOrDefault();
            
            if(p.MainLanguageID == 0)
            {
                p.MainLanguageID = _context.Languages.Where(x => x.Name == "Kingdom Common").Select(X => X.LanguageID).FirstOrDefault();
            }

            foreach(var skill in vm.skills)
            {
                PlayerSkill ps = new PlayerSkill();
                ps.SkillID = skill.skillID;
                ps.LivingID = p.LivingID;
                ps.Level = (double)skill.total;
                p.PlayerSkills.Add(ps);

                _context.Add(ps);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var x = 5;
            }

            p.MaxHP = service.SetMaxHP(p);
            p.MaxMP = service.SetMaxMP(p);
            p.CurrentHP = p.MaxHP;
            p.CurrentMP = p.MaxMP;

            _context.Update(p);
            await _context.SaveChangesAsync();

            return RedirectToAction("CharacterManager");
        }

        public async Task<IActionResult> CharacterManager()
        {
            var user = await _userManager.GetUserAsync(User);

            return ViewComponent("CharacterList", new { currentUser = user });
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PlayerCharacters.Include(p => p.Race).Include(p => p.Background).Include(p => p.RoomKingdom).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerCharacter = await _context.PlayerCharacters
                .Include(p => p.Race)
                .Include(p => p.Background)
                .Include(p => p.RoomKingdom)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.LivingID == id);
            if (playerCharacter == null)
            {
                return NotFound();
            }

            return View(playerCharacter);
        }

        public IActionResult Create()
        {
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Description");
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "PlayerBackgroundID");
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "RoomKingdomID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerCharacter playerCharacter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playerCharacter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Description", playerCharacter.RaceID);
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "PlayerBackgroundID", playerCharacter.PlayerBackgroundID);
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "RoomKingdomID", playerCharacter.RoomKingdomID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", playerCharacter.UserID);
            return View(playerCharacter);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerCharacter = await _context.PlayerCharacters.FindAsync(id);
            if (playerCharacter == null)
            {
                return NotFound();
            }
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Description", playerCharacter.RaceID);
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "PlayerBackgroundID", playerCharacter.PlayerBackgroundID);
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "RoomKingdomID", playerCharacter.RoomKingdomID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", playerCharacter.UserID);
            return View(playerCharacter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerCharacter playerCharacter)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playerCharacter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerCharacterExists(playerCharacter.LivingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RaceID"] = new SelectList(_context.Races, "RaceID", "Description", playerCharacter.RaceID);
            ViewData["PlayerBackgroundID"] = new SelectList(_context.PlayerBackgrounds, "PlayerBackgroundID", "PlayerBackgroundID", playerCharacter.PlayerBackgroundID);
            ViewData["RoomKingdomID"] = new SelectList(_context.RoomKingdoms, "RoomKingdomID", "RoomKingdomID", playerCharacter.RoomKingdomID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", playerCharacter.UserID);
            return View(playerCharacter);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerCharacter = await _context.PlayerCharacters
                .Include(p => p.Race)
                .Include(p => p.Background)
                .Include(p => p.RoomKingdom)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.LivingID == id);
            if (playerCharacter == null)
            {
                return NotFound();
            }

            return View(playerCharacter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerCharacter = await _context.PlayerCharacters.FindAsync(id);
            _context.PlayerCharacters.Remove(playerCharacter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerCharacterExists(int id)
        {
            return _context.PlayerCharacters.Any(e => e.LivingID == id);
        }
    }
}
