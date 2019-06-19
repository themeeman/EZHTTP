using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZHTTP {
    class Forms {
        public static Dictionary<string, string> ParseForm(string query) {
            var p = HttpUtility.ParseQueryString(query);
            return p.AllKeys.ToDictionary(k => k, k => p[k]);
        }
        public static T ParseForm<T>(string query) where T : new() {
            T result = new T();
            var fields = typeof(T).GetFields();

            //fields[0].
            return result;
        }
    }
}
