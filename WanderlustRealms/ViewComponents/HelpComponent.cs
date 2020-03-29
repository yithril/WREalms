using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models.Help;

namespace WanderlustRealms.ViewComponents
{
    public class HelpComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public HelpComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string HelpTerm)
        {
            if(_context.Races.Any(x => x.Name == HelpTerm))
            {
                HelpItem h = new HelpItem();
                h.HelpDescription = _context.Races.Where(x => x.Name == HelpTerm).Select(x => x.Description).FirstOrDefault();
                h.HelpTerm = HelpTerm;
                return View(h);
            }

            if(_context.PlayerBackgrounds.Any(x => x.Name == HelpTerm))
            {
                HelpItem h = new HelpItem();
                h.HelpDescription = _context.PlayerBackgrounds.Where(x => x.Name == HelpTerm).Select(x => x.Description).FirstOrDefault();
                h.HelpTerm = HelpTerm;
                return View(h);
            }

            if(_context.Skills.Any(x => x.Name == HelpTerm))
            {
                HelpItem h = new HelpItem();
                h.HelpDescription = _context.Skills.Where(x => x.Name == HelpTerm).Select(x => x.Description).FirstOrDefault();
                h.HelpTerm = HelpTerm;
                return View(h);
            }

            return View(_context.HelpItems.Where(x => x.HelpTerm == HelpTerm).FirstOrDefault());
        }
    }
}
