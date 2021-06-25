using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console {
    public sealed class ColoredString {
        public string Text { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.White;



        public ColoredString(string text) => Text = text;
        public ColoredString(string text, ConsoleColor color) : this(text) => Color = color;

    }
}
