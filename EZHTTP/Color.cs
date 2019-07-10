#nullable enable

using System;
using System.Text.RegularExpressions;

namespace EZHTTP {
    public struct Color : IEquatable<Color> {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public static explicit operator (byte Red, byte Green, byte Blue)(Color c) {
            return (c.Red, c.Green, c.Blue);
        }

        public (byte Red, byte Green, byte Blue) AsTuple() {
            return ((byte Red, byte Green, byte Blue))this;
        }

        public Color(byte Red, byte Green, byte Blue) {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
        }

        public static Color Parse(string s) {
            if (!Regex.Match(s, @"^#[\dabcdefABCDEF]{6}$").Success) {
                throw new ParseException(s, "#rrggbb");
            }
            return new Color(
                Red: Convert.ToByte(s.Substring(1, 2), 16),
                Green: Convert.ToByte(s.Substring(3, 2), 16),
                Blue: Convert.ToByte(s.Substring(5, 2), 16)
            );
        }

        public override bool Equals(object obj) {
            if (obj is null)
                return false;

            return Equals((Color)obj);
        }

        public bool Equals(Color other) {
            return Red == other.Red &&
                   Green == other.Green &&
                   Blue == other.Blue;
        }

        public override int GetHashCode() {
            var hashCode = -1058441243;
            hashCode = hashCode * -1521134295 + Red.GetHashCode();
            hashCode = hashCode * -1521134295 + Green.GetHashCode();
            hashCode = hashCode * -1521134295 + Blue.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Color left, Color right) {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right) {
            return !(left == right);
        }
    }
}
