using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Skills;
using WanderlustRealms.Models.ViewModel;

namespace WanderlustRealms.ViewComponents
{
    public class SkillCalculatorViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public SkillCalculatorViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int RaceID, int PlayerBackgroundID = 0)
        {
            List<Skill> mReturn = new List<Skill>();

            if(PlayerBackgroundID == 0)
            {
                var background = _context.RaceBackgrounds.Where(x => x.RaceID == RaceID).Select(x => x.PlayerBackground).OrderBy(x => x.Name).FirstOrDefault();
                PlayerBackgroundID = background.PlayerBackgroundID;
                ViewBag.BackgroundName = background.Name;
            }
            else
            {
                ViewBag.BackgroundName = _context.PlayerBackgrounds.Where(x => x.PlayerBackgroundID == PlayerBackgroundID).Select(x => x.Name).FirstOrDefault();
            }
            

            //Get all skills
            mReturn.AddRange(_context.Skills.Where(x => x.IsActive && !x.IsBackgroundSpecific).ToList());
            var raceList = _context.RaceSkills.Where(x => x.RaceID == RaceID).Include(x => x.Skill).ToList();
            mReturn.AddRange(raceList.Select(x => x.Skill));
            var backgroundList = _context.BackgroundSkills.Where(x => x.PlayerBackgroundID == PlayerBackgroundID).Include(x => x.Skill).ToList();
            mReturn.AddRange(backgroundList.Select(x => x.Skill).ToList());

            //Get rid of duplicates
            mReturn = mReturn.Distinct().ToList();
            List<SkillPointViewModel> spvm = new List<SkillPointViewModel>();

            //Prep view model
            foreach (var s in mReturn)
            {
                SkillPointViewModel vm = new SkillPointViewModel();
                vm.Name = s.Name;

                if (backgroundList.Select(x => x.Skill).ToList().Contains(s))
                {
                    vm.BackgroundBonus = 20;
                }

                if (raceList.Select(x => x.Skill).ToList().Contains(s))
                {
                    vm.RaceBonus = raceList.Where(x => x.SkillID == s.SkillID).Select(x => x.StartingBonus).FirstOrDefault();
                }

                vm.SkillID = s.SkillID;

                spvm.Add(vm);
            }

            //Get starting skill points
            ViewBag.StartingSkillPoints = _context.Races.Where(x => x.RaceID == RaceID).Select(x => x.SkillPoints).FirstOrDefault();
            ViewBag.PlayerBackgroundList = new SelectList(_context.RaceBackgrounds.Where(x => x.RaceID == RaceID).Select(x => x.PlayerBackground).OrderBy(x => x.Name), "PlayerBackgroundID", "Name", PlayerBackgroundID);

            return View(spvm.OrderBy(x => x.Name).ToList());
        }
    }
}
