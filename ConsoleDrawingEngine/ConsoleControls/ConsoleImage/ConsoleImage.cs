using System;
using System.Collections.Generic;
using System.Drawing;
using Game.ConsoleDrawingEngine.Types;

namespace Game.ConsoleDrawingEngine.Controls {
    public sealed class ConsoleImage : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; protected set; }



        public ConsoleImage(Point location, ConsolePicture consolePicture) : base(location, consolePicture.Picture.Size) {
            ConsolePicture = consolePicture;
        }

    }
}
