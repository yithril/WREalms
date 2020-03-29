using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models;

namespace WanderlustRealms.ViewComponents
{
    public class Chat : ViewComponent
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Chat(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int LivingID)
        {
            ViewBag.LivingID = LivingID;
            ViewBag.Name = _context.PlayerCharacters.Where(x => x.LivingID == LivingID).Select(x => x.Name).FirstOrDefault();
            return View();
        }
    }
}
