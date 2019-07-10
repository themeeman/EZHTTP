using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EZHTTP {
    public class ParseException : Exception {
        public ParseException(string value, string form) : 
            base($"Cannot parse {value} as it is not in the form {form}") {}
    }

    public static class Forms {
        public static Dictionary<string, string> Parse(string query) {
            var p = HttpUtility.ParseQueryString(query);
            return p.AllKeys.ToDictionary(k => k, k => p[k], StringComparer.OrdinalIgnoreCase);
        }

        public static T Parse<T>(string query) where T : new() {
            object result = new T();
            var dict = Parse(query);
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields) {
                if (dict.TryGetValue(field.Name, out string value)) {
                    if (field.FieldType.IsEnum) {
                        try {
                            field.SetValue(result, Enum.Parse(field.FieldType, value, ignoreCase: true));
                        } catch {
                            throw;
                        }
                    } else if (field.FieldType == typeof(string)) {
                        field.SetValue(result, value);
                    }
                }
            }
            foreach (var prop in props) {
                if (dict.TryGetValue(prop.Name, out string value)) {
                    prop.SetValue(result, value);
                }
            }
            return (T)result;
        }
    }
}
