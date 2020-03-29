using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Data;
using WanderlustRealms.Models;
using Microsoft.EntityFrameworkCore;

namespace WanderlustRealms.ViewComponents
{
    public class CharacterDetails : ViewComponent
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CharacterDetails(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int LivingID)
        {
            return View(await _context.PlayerCharacters.Where(x => x.LivingID == LivingID).Include(x => x.Race).Include(x => x.Background).Include(x => x.RoomKingdom).Include(x => x.PlayerSkills).ThenInclude(y => y.Skill).FirstOrDefaultAsync());
        }
    }
}
