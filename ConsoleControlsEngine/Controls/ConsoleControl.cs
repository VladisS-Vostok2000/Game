using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Game.ColoredCharsEngine;

namespace Game.ConsoleControlsEngine {
    public abstract class ConsoleControl {
        public int X { get; set; }
        public int Y { get; set; }
        public Point Location { get; set; }
        public abstract Size Size { get; }



        protected ConsoleControl(Point location) {
            Location = location;
        }



        public abstract void VisualizeInConsole();

    }
}
