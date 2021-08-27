using System;
using System.Collections.Generic;
using System.Drawing;
using Game.ColoredCharsEngine.Types.Pictures;
using Game.ConsoleDrawingEngine.Types;
using Game.ExtensionMethods;

namespace Game.ConsoleDrawingEngine.Controls {
    public class ConsolePictureControl : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; }



        public ConsolePictureControl(Point location, ConsolePicture consolePicture) : base(location) {
            ConsolePicture = consolePicture;
        }

    }
}
