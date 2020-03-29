using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Services
{
    public class TrainerService
    {
        private readonly ApplicationDbContext _context;

        public TrainerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<string> GetTrainerSkills(int LivingID)
        {
            return _context.TrainerSkills.Where(x => x.LivingID == LivingID).Include(x => x.Skill).OrderBy(x => x.Skill.Name).Select(x => x.Skill.Name).ToList();
        }

        public int CalculateXPCost(int LivingID, string skill, PlayerCharacter pc)
        {
            var skills = _context.TrainerSkills.Where(x => x.LivingID == LivingID).Include(x => x.Skill).Where(x => x.Skill.Name == skill).FirstOrDefault();

            var currentLevel = pc.PlayerSkills.Where(x => x.Skill.Name == skill).Select(x => x.Level).FirstOrDefault();

            return (int)Math.Ceiling(Math.Pow((double)(currentLevel / 5), 2) + 150);
        }

        public int CalculateGoldCost(int LivingID, string skill, PlayerCharacter pc)
        {
            var skills = _context.TrainerSkills.Where(x => x.LivingID == LivingID).Include(x => x.Skill).Where(x => x.Skill.Name == skill).FirstOrDefault();

            var currentLevel = pc.PlayerSkills.Where(x => x.Skill.Name == skill).Select(x => x.Level).FirstOrDefault();

            return (int)Math.Ceiling(Math.Pow((double)(currentLevel / 2), 2) + 50);
        }

        public bool SkillInTrainerSkillRange(int LivingID, string skill, PlayerCharacter pc)
        {
            var targetSkill = _context.TrainerSkills.Where(x => x.LivingID == LivingID).Include(x => x.Skill).Where(x => x.Skill.Name == skill).FirstOrDefault();

            if(pc.PlayerSkills.Any(x => x.Skill.Name == targetSkill.Skill.Name))
            {
                var pSkill = pc.PlayerSkills.Where(x => x.Skill.Name == targetSkill.Skill.Name).FirstOrDefault();

                if(pSkill.Level < targetSkill.MinLevel || pSkill.Level > targetSkill.MaxLevel)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public bool TrainerPreReqsMet(int LivingID, PlayerCharacter pc)
        {
            var mReturn = true;

            var preReqs = _context.TrainerPreReqs.Where(x => x.LivingID == LivingID).ToList();

            if(preReqs != null)
            {
                foreach(var p in preReqs)
                {
                    if(p.PlayerBackgroundID != null)
                    {
                        if(pc.PlayerBackgroundID != p.PlayerBackgroundID)
                        {
                            return false;
                        }
                    }

                    if(p.QuestID != null)
                    {

                        if(!_context.PlayerQuests.Any(x => x.LivingID == pc.LivingID && x.QuestID == p.QuestID && x.IsComplete))
                        {
                            return false;
                        }
                    }

                    if(p.QuestPoints != null)
                    {
                        if(p.QuestPoints > _context.PlayerQuests.Where(x => x.LivingID == pc.LivingID && x.IsComplete).Include(x => x.Quest).Sum(x => x.Quest.QuestPoints))
                        {
                            return false;
                        }
                    }

                    if(p.RaceID != null)
                    {
                        if(p.RaceID != pc.Race.RaceID)
                        {
                            return false;
                        }
                    }
                }
            }

            return mReturn;
        }

        public string TrainerRejectSpeech(int LivingID, PlayerCharacter pc)
        {
            var preReqs = _context.TrainerPreReqs.Where(x => x.LivingID == LivingID).ToList();

            if (preReqs != null)
            {
                foreach (var p in preReqs)
                {
                    if (p.PlayerBackgroundID != null)
                    {
                        if (pc.PlayerBackgroundID != p.PlayerBackgroundID)
                        {
                            var name = _context.PlayerBackgrounds.Where(x => x.PlayerBackgroundID == p.PlayerBackgroundID).Select(x => x.Name).FirstOrDefault();
                            return "I only train those who have been " + name + " in the past.  That isn't you.";
                        }
                    }

                    if (p.QuestID != null)
                    {

                        if (!_context.PlayerQuests.Any(x => x.LivingID == pc.LivingID && x.QuestID == p.QuestID && x.IsComplete))
                        {
                            return "Who are you and why would I train you?";
                        }
                    }

                    if (p.QuestPoints != null)
                    {
                        if (p.QuestPoints > _context.PlayerQuests.Where(x => x.LivingID == pc.LivingID && x.IsComplete).Include(x => x.Quest).Sum(x => x.Quest.QuestPoints))
                        {
                            return "You're a little too green to train.  Go out and adventure more.";
                        }
                    }

                    if (p.RaceID != null)
                    {
                        if (p.RaceID != pc.Race.RaceID)
                        {
                            var name = _context.Races.Where(x => x.RaceID == p.RaceID).Select(x => x.Name).FirstOrDefault();
                            return "I don't train your kind.  I only train " + name + ".";
                        }
                    }
                }
            }

            return "";
        }
    }
}
