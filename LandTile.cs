using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public struct Landtile : IConsoleDrawable {
        public ConsoleImage ConsoleImage { get; set; }
        public ConsoleColor Color { get => ConsoleImage.Color; set => new ConsoleImage(ConsoleChar, value); }
        public char ConsoleChar { get => ConsoleImage.Char; set => new ConsoleImage(value, Color); }
        public string Name { get; set; }
        public string DisplayedName { get; set; }



        public Landtile(string name, string displayedName, ConsoleImage consoleImage) {
            Name = name;
            DisplayedName = displayedName;
            ConsoleImage = consoleImage;
        }

    }
}
