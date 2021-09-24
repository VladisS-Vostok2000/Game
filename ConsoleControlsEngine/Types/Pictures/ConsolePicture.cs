using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine;

namespace Game.ConsoleControlsEngine.Types {
    public abstract class ConsolePicture {
        public Picture Picture { get; }



        protected ConsolePicture(Picture picture) {
            Picture = picture;
        }



        public abstract void VisualizeInConsole(Point location);

    }
}
