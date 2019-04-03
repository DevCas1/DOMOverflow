using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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


        public static IEnumerable<T> AddStateless<T>(this IEnumerable<T> list, T elem) {
            List<T> newlist = new List<T>(list);
            newlist.Add(elem);
            return newlist;
        }


        public static string GetWebsiteBaseURL(this HttpRequestBase request) {
            return request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/";
        }


        public static void RedirectWithPost(this HttpResponseBase response, NameValueCollection data, string url) {
            response.Clear();

            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", url);

            foreach (string key in data) s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", key, data[key]);
            
            s.Append("</form></body></html>");
            response.Write(s.ToString());

            response.End();
        }
    }
}