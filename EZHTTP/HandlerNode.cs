using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EZHTTP {
    public class HandlerNode {
        public Action<HttpListenerContext> Callback { get; set; }
        public Dictionary<string, HandlerNode> ChildNodes { get; }
        public bool IsFixed { get; set; }

        public HandlerNode() {
            Callback = null;
            ChildNodes = new Dictionary<string, HandlerNode>();
            IsFixed = true;
        }

        public HandlerNode(Action<HttpListenerContext> callback, bool isFixed = true) {
            Callback = callback;
            ChildNodes = new Dictionary<string, HandlerNode>();
            IsFixed = isFixed;
        }

        public HandlerNode GetNode(string pattern) {
            var handler = this;
            foreach (string s in pattern.Split('/').Select(s => s.ToLower()).Where(s => s.Length > 0)) {
                if (handler.ChildNodes.ContainsKey(s)) {
                    handler = handler.ChildNodes[s];
                }
                else if (handler.IsFixed) {
                    return null;
                }
                else {
                    return handler;
                }
            }
            return handler;
        }
    }
}
