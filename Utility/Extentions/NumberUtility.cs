using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utility
{
    public static class NumberUtility
    {
        public static string ToEnglishNumber(this string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return mobile;

            var response = mobile.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3")
                .Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
            return response;
        }
    }
}
