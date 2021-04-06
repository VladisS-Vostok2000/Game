using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Unit : IConsoleDrawable {
        public ConsoleImage ConsoleImage { get; set; }

        public string Name { get; }

        public Point Location { get; }



        public Unit(string name, Point location, ConsoleImage consoleImage) {
            Name = name;
            Location = location;
            ConsoleImage = consoleImage;
        }

    }
}
