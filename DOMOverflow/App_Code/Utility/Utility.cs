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
    }
}