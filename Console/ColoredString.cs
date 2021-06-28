using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console {
    public sealed class ColoredString {
        public string Text { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        public int Length { get; internal set; }

        public ColoredString(string text, ConsoleColor color = ConsoleColor.White) => Color = color;



        public ConsolePixel this[int index] => new ConsolePixel(Text[index], Color);



        public int IndexOf(string value) => Text.IndexOf(value);
        public int IndexOfNewLine() => Text.IndexOfNewLine();
        public int IndexOfNewLine(int startIndex) => Text.IndexOfNewLine(startIndex);
        public string Substring(int startIndex) => Text.Substring(startIndex);
        public string Substring(int startIndex, int length) => Text.Substring(startIndex, length);
        public ColoredString ColoredSubstring(int startIndex) => new ColoredString(Text.Substring(startIndex), Color);
        public ColoredString ColoredSubstring(int startIndex, int length) => new ColoredString(Text.Substring(startIndex, length), Color);

    }
}
