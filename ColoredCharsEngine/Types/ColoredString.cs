using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Game.BasicTypesLibrary;

namespace Game.ColoredCharsEngine {
    /// <summary>
    /// Цветная строка.
    /// Не может принимать перенос строки.
    /// </summary>
    public sealed class ColoredString {
        public string Text { get; }
        public ConsoleColor Color { get; set; }
        public int Length { get; }



        /// <summary>
        /// Создаст экземпляр <see cref="ColoredString"/>.
        /// Не может содержать переносов строк.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public ColoredString(string text, ConsoleColor color = ConsoleColor.White) {
            if (text.IndexOfNewLine() != -1) {
                throw new FormatException($"{nameof(ColoredString)} не может содержать перенос строки.");
            }

            Text = text;
            Color = color;
            Length = text.Length;
        }
        /// <summary>
        /// Создаст экземпляр <see cref="ColoredString"/>.
        /// Не может содержать переносов строк.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public ColoredString(char chr, int length, ConsoleColor color = ConsoleColor.White) : this(new string(chr, length), color) {

        }



        public ColoredChar this[int index] => new ColoredChar(Text[index], Color);



        public static explicit operator ColoredString(string v1) {
            return v1.ToColoredString();
        }
        /// <summary>
        /// Создаст экземпляр, элементы заданного <see cref="string"/> которые будут в начале возвращаемого <see cref="ColoredString"/>.
        /// </summary>
        public static ColoredString operator +(string v1, ColoredString v2) {
            return new ColoredString(v1 + v2.Text);
        }
        /// <summary>
        /// Создаст экземпляр, элементы заданного <see cref="string"/> которые будут в конце возвращаемого <see cref="ColoredString"/>.
        /// </summary>
        public static ColoredString operator +(ColoredString v2, string v1) {
            return new ColoredString(v2.Text + v1);
        }



        public int IndexOf(string value) => Text.IndexOf(value);
        public string Substring(int startIndex) => Text.Substring(startIndex);
        public string Substring(int startIndex, int length) => Text.Substring(startIndex, length);
        public ColoredString ColoredSubstring(int startIndex) => new ColoredString(Text.Substring(startIndex), Color);
        public ColoredString ColoredSubstring(int startIndex, int length) => new ColoredString(Text.Substring(startIndex, length), Color);
        public override string ToString() => Text;

    }
}
