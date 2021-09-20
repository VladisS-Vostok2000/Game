using Game.BasicTypesLibrary.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.BasicTypesLibrary.ExtensionMethods.BasicTypesExtensionsMethods;

namespace Game.ColoredCharsEngine {
    /// <summary>
    /// Цветная строка.
    /// Не может принимать перенос строки.
    /// </summary>
    public sealed class ColoredString {
        private readonly string text;
        public string Text { get; }
        public ConsoleColor Color { get; set; }
        public int Length => Text.Length;



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
        }
        /// <summary>
        /// Создаст экземпляр <see cref="ColoredString"/>.
        /// Не может содержать переносов строк.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public ColoredString(char chr, int length, ConsoleColor color = ConsoleColor.White) : this(new string(chr, length), color) {

        }



        public ColoredChar this[int index] => new ColoredChar(Text[index], Color);



        public int IndexOf(string value) => Text.IndexOf(value);
        public string Substring(int startIndex) => Text.Substring(startIndex);
        public string Substring(int startIndex, int length) => Text.Substring(startIndex, length);
        public ColoredString ColoredSubstring(int startIndex) => new ColoredString(Text.Substring(startIndex), Color);
        public ColoredString ColoredSubstring(int startIndex, int length) => new ColoredString(Text.Substring(startIndex, length), Color);
        public override string ToString() => Text;

    }
}
