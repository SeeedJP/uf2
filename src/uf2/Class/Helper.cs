using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace uf2
{
    public class Helper
    {
        public static bool ConvertToUint32(string str, out uint value)
        {
            value = 0;
            if (string.IsNullOrEmpty(str)) return false;
            if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (!uint.TryParse(str[2..], System.Globalization.NumberStyles.HexNumber, null, out value)) return false;
            }
            else
            {
                if (!uint.TryParse(str, out value)) return false;
            }

            return true;
        }

    }
}
