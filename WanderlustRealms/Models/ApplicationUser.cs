using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WanderlustRealms.Models.Living;

namespace WanderlustRealms.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<PlayerCharacter> PlayerCharacters { get; set; }
        public bool IsPremium { get; set; }
    }
}
