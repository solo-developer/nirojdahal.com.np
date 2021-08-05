using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Personal.Domain.Helpers
{
    public static class EnumUtils
    {
        public static IEnumerable<TEnum> GetAllValues<TEnum>()
       where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
