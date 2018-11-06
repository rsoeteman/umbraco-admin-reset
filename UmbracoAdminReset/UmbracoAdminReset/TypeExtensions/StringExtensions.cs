using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbracoAdminReset.TypeExtensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string s)
        {
            int.TryParse(s, out int i);
            return i;
        }
    }
}
