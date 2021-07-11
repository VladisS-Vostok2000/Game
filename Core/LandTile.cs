using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    // REFACTORING: Разве это не должен быть класс?
    // Я должен быть уверен в том, что, если я что-то в нём изменю,
    // это отразится везде.
    public struct Landtile {
        public string Name { get; set; }
        public string DisplayedName { get; set; }


        public ColoredChar ColoredChar { get; set; }
        public ConsoleColor Color {
            get => ColoredChar.Color;
            set => ColoredChar = new ColoredChar(ColoredChar.Char, value);
        }
        public char Char {
            get => ColoredChar.Char;
            set => ColoredChar = new ColoredChar(value, ColoredChar.Color);
        }



        public Landtile(string name, string displayedName, char character, ConsoleColor color) {
            Name = name;
            DisplayedName = displayedName;
            ColoredChar = new ColoredChar(character, color);
        }

    }
}
