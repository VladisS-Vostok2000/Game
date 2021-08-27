using Game.ConsoleDrawingEngine.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine.Pictures;
using static Game.ConsoleDrawingEngine.ConsoleDrawing;
using static Game.BasicTypesLibrary.ExtensionMethods.BasicTypesExtensionsMethods;
using static Game.BasicTypesLibrary.ExtensionMethods.ConsoleExtensionMethods;
using static System.Console;
using Game.ConsoleDrawingEngine.Types.Pictures;

namespace Game.ConsoleDrawingEngine.ConsoleControls {
    public sealed class ConsoleVoidControl : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; }



        public ConsoleVoidControl(Point location, Size size) : base(location) {
            ConsolePicture = new ConsoleSingleCharPicture(new SingleCharPicture(size ,' '));
        }

    }
}
