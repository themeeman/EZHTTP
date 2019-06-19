using System.IO;
using System.Text;

namespace EZHTTP {
    static class Utility {
        public static void Write(this Stream stream, byte[] bytes) {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteString(this Stream stream, string s) {
            stream.WriteString(s, Encoding.ASCII);
        }

        public static void WriteString(this Stream stream, string s, Encoding encoding) {
            stream.Write(encoding.GetBytes(s));
        }
    }
}
