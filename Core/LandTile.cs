using Game.ConsoleDrawingEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine;

namespace Game.Core {
    // TASK: Пусть это станет классом.
    // Если появится необходимость добавить тайлу состояние,
    // будем использовать агрегирование.
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
