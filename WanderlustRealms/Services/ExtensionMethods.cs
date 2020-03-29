using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanderlustRealms.Services
{
    public class ExtensionMethods {
        public static string GetParamName(System.Reflection.MethodInfo method, int index)
        {
            string retVal = string.Empty;

            if (method != null && method.GetParameters().Length > index)
            {
                retVal = method.GetParameters()[index].Name;
            }

            return retVal;
        }
    }
}
