using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Services
{
    public class PlayerCharacterService
    {
        private readonly ApplicationDbContext _context;

        public PlayerCharacterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int SetMaxMP(PlayerCharacter p)
        {
            p.PlayerSkills = _context.PlayerSkills.Where(x => x.LivingID == p.LivingID).Include(x => x.Skill).ToList();
            var ev = p.PlayerSkills.Where(x => x.Skill.Name == "Evanescence").FirstOrDefault();
            var modifier = 0.0;

            if(ev != null)
            {
                modifier =  ev.Level / 2;
            }
            
            return Convert.ToInt32(((p.Willpower * .75) + p.Intellect) + modifier);
        }

        public int SetMaxHP(PlayerCharacter p)
        {
            p.PlayerSkills = _context.PlayerSkills.Where(x => x.LivingID == p.LivingID).Include(x => x.Skill).ToList();
            var ev = p.PlayerSkills.Where(x => x.Skill.Name == "Hardiness").FirstOrDefault();
            var modifier = 0.0;

            if (ev != null)
            {
                modifier = ev.Level / 2;
            }

            return Convert.ToInt32(p.Durability + (p.Willpower * .5) + modifier);
        }
    }
}
