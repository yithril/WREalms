using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Living;
using WanderlustRealms.Models.Skills;

namespace WanderlustRealms.Services
{
    public class SkillService
    {
        public string TwoHandedFightingMessage(PlayerCharacter pc)
        {
            var message = "";

            var thf = pc.PlayerSkills.Where(X => X.Skill.Name == "Dual Wield").FirstOrDefault();

            if(thf == null)
            {
                message = "You have no idea how to effectively wield two weapons.";
            }
            else
            {
                if (thf.Level < 21)
                {
                    message = "You feel extremely awkward wielding two weapons.";
                }

                if (thf.Level > 20 && thf.Level < 41)
                {
                    message = "You feel very awkward wielding two weapons.";
                }

                if (thf.Level > 40 && thf.Level < 61)
                {
                    message = "You feel somewhat awkward wielding two weapons.";
                }

                if (thf.Level > 60)
                {
                    message = "You feel comfortable wielding two weapons.";
                }

            }

            return message;
        }

        public string GetSkillAdjective(PlayerSkill skill)
        {
            if(skill.Level < 20)
            {
                return "Novice";
            }else if(skill.Level > 19 && skill.Level < 31)
            {
                return "Inept";
            }else if(skill.Level > 30 && skill.Level < 41)
            {
                return "Unskilled";
            }
            else if (skill.Level > 40 && skill.Level < 51)
            {
                return "Average";
            }
            else if(skill.Level > 50 && skill.Level < 61)
            {
                return "Above Average";
            }else if(skill.Level > 60 && skill.Level < 71)
            {
                return "Skilled";
            }else if(skill.Level > 70 && skill.Level < 81)
            {
                return "Very Skilled";
            }else if(skill.Level > 80 && skill.Level < 91)
            {
                return "Competent";
            }
            else if (skill.Level > 90 && skill.Level < 101)
            {
                return "Proficient";
            }
            else if (skill.Level > 100 && skill.Level < 111)
            {
                return "Experienced";
            }
            else if (skill.Level > 110 && skill.Level < 121)
            {
                return "Adept";
            }
            else if (skill.Level > 120 && skill.Level < 131)
            {
                return "Very Adept";
            }
            else if (skill.Level > 130 && skill.Level < 141)
            {
                return "Versed";
            }
            else if (skill.Level > 140 && skill.Level < 151)
            {
                return "Prodigious";
            }
            else if (skill.Level > 150 && skill.Level < 161)
            {
                return "Masterful";
            }
            else if (skill.Level > 160 && skill.Level < 171)
            {
                return "Adroit";
            }
            else if (skill.Level > 170 && skill.Level < 181)
            {
                return "Monumental";
            }
            else if (skill.Level > 180 && skill.Level < 191)
            {
                return "Preturnatural";
            }
            else if (skill.Level > 190 && skill.Level < 201)
            {
                return "Otherwordly";
            }
            else
            {
                return "Divine";
            }

        }
    }
}
