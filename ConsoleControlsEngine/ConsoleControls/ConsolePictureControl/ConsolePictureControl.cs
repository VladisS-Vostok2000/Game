using System;
using System.Collections.Generic;
using System.Drawing;
using Game.ColoredCharsEngine.Types.Pictures;
using Game.ConsoleControlsEngine.Types;
using Game.BasicTypesLibrary.Extensions;

namespace Game.ConsoleControlsEngine.Controls {
    public class ConsolePictureControl : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; }



        public ConsolePictureControl(Point location, ConsolePicture consolePicture) : base(location) {
            ConsolePicture = consolePicture;
        }

    }
}
