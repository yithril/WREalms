using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models;

namespace WanderlustRealms.ViewComponents
{
    public class CharacterList : ViewComponent
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CharacterList(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser currentUser)
        {
            var charCount = _context.PlayerCharacters.Where(x => x.UserID == currentUser.Id).Count();
            var maxCount = 4;
            ViewBag.CanCreate = false;

            if (currentUser.IsPremium)
            {
                maxCount = 10;
            }

            if (charCount < maxCount)
            {
                ViewBag.CanCreate = true;
            }

            return View(_context.PlayerCharacters.Where(x => x.UserID == currentUser.Id).Include(x => x.Race).Include(x => x.RoomKingdom).ToList());
        }
    }
}
