using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.Living
{
    public class Language
    {
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public string Adjective { get; set; }
        public string Adjective2 { get; set; }
        public double LanguageModifier { get; set; }
    }
}
