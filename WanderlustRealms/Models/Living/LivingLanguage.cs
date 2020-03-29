using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Models.Living
{
    public class LivingLanguage
    {
        public int LivingID { get; set; }
        public Living Living { get; set; }
        public int LanguageID { get; set; }
        public Language Language { get; set; }
        public double Level { get; set; }
    }
}
