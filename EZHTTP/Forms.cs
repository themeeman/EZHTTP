using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZHTTP {
    public static class Forms {
        public static Dictionary<string, string> ParseForm(string query) {
            var p = HttpUtility.ParseQueryString(query);
            return p.AllKeys.ToDictionary(k => k, k => p[k], StringComparer.OrdinalIgnoreCase);
        }
        public static T ParseForm<T>(string query) where T : new() {
            T result = new T();
            var fields = typeof(T).GetFields();
            var dict = ParseForm(query);
            foreach (var field in fields) {
                if (dict.TryGetValue(field.Name, out string value)) {
                    field.SetValue(result, value);
                }
            }
            return result;
        }
    }
}
