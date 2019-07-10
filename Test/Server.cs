#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EZHTTP;

class Server {
    public static void Main() {
        var mux = new HttpMux(":9090");
        mux.Handle("/", context => context.Response.OutputStream.Write("Root Page\n"));
        mux.Handle("/aaa/", context => context.Response.OutputStream.Write("aaa"));
        mux.Start();
        Console.ReadKey();
        mux.Stop();
    }
}