using Game.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine {
    public sealed class ColoredString {
        public string Text { get; set; }
        public ConsoleColor Color { get; set; }
        public int Length => Text.Length;



        public ColoredString(string text, ConsoleColor color = ConsoleColor.White) {
            Text = text;
            Color = color;
        }



        public ColoredChar this[int index] => new ColoredChar(Text[index], Color);



        public int IndexOf(string value) => Text.IndexOf(value);
        public int IndexOfNewLine() => Text.IndexOfNewLine();
        public int IndexOfNewLine(int startIndex) => Text.IndexOfNewLine(startIndex);
        public string Substring(int startIndex) => Text.Substring(startIndex);
        public string Substring(int startIndex, int length) => Text.Substring(startIndex, length);
        public ColoredString ColoredSubstring(int startIndex) => new ColoredString(Text.Substring(startIndex), Color);
        public ColoredString ColoredSubstring(int startIndex, int length) => new ColoredString(Text.Substring(startIndex, length), Color);
        public override string ToString() => Text;

    }
}
