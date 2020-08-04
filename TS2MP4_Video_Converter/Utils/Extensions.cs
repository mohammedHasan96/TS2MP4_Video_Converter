using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Extensions
    {
        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        public static string SizeFormat(this long value)
        {
            int counter = 0;
            decimal number = (decimal)value;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

        public static void Append(this string obj, string value)
            => obj += value;

        public static void AppendLine(this string obj, string value)
            => obj.Append($"\n{value}");
    }
}
