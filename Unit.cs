using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Unit : IConsoleDrawable {
        public ConsoleImage ConsoleImage { get; set; }
        public string DisplayedName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }

        public Point Location { get; set; }

        public Body Body { get; set; }
        public Chassis Chassis { get; set; }


    }
}
