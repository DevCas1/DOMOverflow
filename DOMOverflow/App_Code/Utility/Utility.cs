using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DOMOverflow {
    public static class Utility {
        public static string ToHexString(this IEnumerable<byte> bytes) {
            StringBuilder builder = new StringBuilder(bytes.Count() * 2);
            foreach (byte b in bytes) builder.AppendFormat("{0:x2}", b);

            return builder.ToString();
        }


        public static HashSet<T> RemoveDuplicates<T>(this IEnumerable<T> obj) {
            return new HashSet<T>(obj);
        }


        public static IEnumerable<string> SplitAll(this IEnumerable<string> strings, char seperator = '\n') {
            List<string> result = new List<string>();
            foreach (string str in strings) result.AddRange(str.Split(seperator));
            return result;
        }


        public static bool ContainsAny(this string str, string chars) {
            foreach (char ch in chars) if (str.Contains(ch)) return true;
            return false;
        }
    }
}