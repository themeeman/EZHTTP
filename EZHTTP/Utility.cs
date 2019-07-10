#nullable enable

using System.IO;
using System.Text;

namespace EZHTTP {
    public static class Utility {
        public static void Write(this Stream stream, byte[] bytes) {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void Write(this Stream stream, string s) {
            stream.Write(s, Encoding.ASCII);
        }

        public static void Write(this Stream stream, string s, Encoding encoding) {
            stream.Write(encoding.GetBytes(s));
        }
    }
}
