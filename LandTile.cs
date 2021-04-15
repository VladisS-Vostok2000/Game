using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public struct Landtile : IConsoleDrawable {
        public string Name { get; set; }
        public string DisplayedName { get; set; }
        public ConsoleImage ConsoleImage { get; set; }



        public Landtile(string name, string displayedName, ConsoleImage consoleImage) {
            Name = name;
            DisplayedName = displayedName;
            ConsoleImage = consoleImage;
        }

    }
}
