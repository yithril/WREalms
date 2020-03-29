using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Races;

namespace WanderlustRealms.ViewComponents
{
    public class StatCalculatorViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public StatCalculatorViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int RaceID = 0)
        {
            var race = new Race();

            if(RaceID == 0)
            {
                race = _context.Races.OrderBy(x => x.Name).FirstOrDefault();
            }
            else
            {
                race = _context.Races.Find(RaceID);
            }

            ViewBag.RaceName = race.Name;
            ViewBag.RaceID = race.RaceID;
            ViewBag.RaceList = new SelectList(_context.Races.OrderBy(x => x.Name), "RaceID", "Name", race.RaceID);

            return View(await _context.Races.Where(x => x.RaceID == race.RaceID).FirstOrDefaultAsync());
        }
    }
}
