using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Interfaces
{
    public interface IDescription
    {
        string GetLongDescription();
        string GetShortDescription();

        string GetName();
    }
}
