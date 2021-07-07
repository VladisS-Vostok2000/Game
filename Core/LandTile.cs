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
    public struct Landtile : IConsoleDrawable {
        public string Name { get; set; }
        public string DisplayedName { get; set; }
        public char Char { get; set; }
        public ConsoleColor Color { get; set; }


        private ColoredChar[,] CharPixels;
        public ConsolePicture ColoredCharPicture { get; }



        public Landtile(string name, string displayedName, char character, ConsoleColor color) {
            Name = name;
            DisplayedName = displayedName;
            Char = character;
            Color = color;
            CharPixels = new ColoredChar[,] { { new ColoredChar(Char, Color) } };
            ColoredCharPicture = new ConsolePicture(CharPixels);
        }

    }
}
