#nullable enable

using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Web;

namespace EZHTTP {
    public struct HttpMux {
        private readonly HandlerNode handlers;
        private HttpListener Listener { get; }
        public static readonly Action<HttpListenerContext> DefaultNotFound =
            context => context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        public Action<HttpListenerContext> NotFound { get; set; }

        public HttpMux(params string[] prefixes) {
            handlers = new HandlerNode();
            Listener = new HttpListener();
            NotFound = DefaultNotFound;
            foreach (string s in prefixes) {
                string ss = s;
                if (ss[0] == ':' && int.TryParse(ss.Substring(1, ss.Length - 1), out int x)) {
                    ss = "localhost" + ss;
                }
                if (ss.Last() != '/') {
                    ss += '/';
                }
                if (!ss.StartsWith("http://") || !ss.StartsWith("https://")) {
                    ss = "http://" + ss;
                }
                AddPrefix(ss);
            }
        }

        public void AddPrefix(string prefix) {
            Listener.Prefixes.Add(prefix);
        }

        public void Handle(string pattern, Action<HttpListenerContext> callback) {
            if (pattern == "/") {
                handlers.Callback = callback;
                return;
            }
            var children = handlers.ChildNodes;
            var segments = pattern.Split('/').Where(s => s.Length > 0).Select(s => s.ToLower()).ToArray();
            foreach (string s in new ArraySegment<string>(segments, 0, segments.Length - 1)) {
                if (!children.ContainsKey(s)) {
                    children[s] = new HandlerNode();
                }
                children = children[s].ChildNodes;
            }
            children[segments.Last()] = new HandlerNode(callback, pattern.Last() != '/');
        }

        public void ServeFile(string pattern, string path) {
            ServeFile(pattern, path, MimeMapping.GetMimeMapping(path));
        }

        public void ServeFile(string pattern, string path, string contentType) {
            Handle(pattern, context => {
                var buffer = File.ReadAllBytes(path);
                context.Response.ContentType = contentType;
                context.Response.OutputStream.Write(buffer);
            });
        }

        private void OnRequest(IAsyncResult result) {
            var listener = result.AsyncState as HttpListener;
            var context = listener!.EndGetContext(result);
            listener.BeginGetContext(OnRequest, listener);
            var handler = handlers.GetNode(context.Request.Url.AbsolutePath);
            if (handler != null && handler.Callback != null) {
                handler.Callback(context);
            } else {
                NotFound(context);
            }
            context.Response.OutputStream.Close();
            context.Request.InputStream.Close();
        }

        public void Start() {
            Listener.Start();
            Listener.BeginGetContext(OnRequest, Listener);
        }

        public void Stop() {
            Listener.Stop();
        }
    }
}
