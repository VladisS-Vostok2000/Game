using Game.ConsoleControlsEngine.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine.Pictures;
using static Game.ConsoleControlsEngine.ConsoleDrawing;
using static Game.BasicTypesLibrary.Extensions.BasicTypesExtensions;
using static System.Console;
using Game.ConsoleControlsEngine.Types.Pictures;

namespace Game.ConsoleControlsEngine.Controls {
    public sealed class ConsoleVoidControl : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; }



        public ConsoleVoidControl(Point location, Size size) : base(location) {
            ConsolePicture = new ConsoleSingleCharPicture(new SingleCharPicture(size ,' '));
        }

    }
}
