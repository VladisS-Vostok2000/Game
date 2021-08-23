using System;
using System.Collections.Generic;
using System.Drawing;
using Game.ColoredCharsEngine.Types.Pictures;
using Game.ConsoleDrawingEngine.Types;
using Game.ExtensionMethods;

namespace Game.ConsoleDrawingEngine.Controls {
    public class ConsolePictureControl : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; protected set; }



        public ConsolePictureControl(Point location, ConsolePicture consolePicture) : base(location, consolePicture.Picture.Size) {
            ConsolePicture = consolePicture;
        }

    }
}
