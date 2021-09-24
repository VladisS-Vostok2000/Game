using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static Game.BasicTypesLibrary.Extensions.BasicTypesExtensions;
using static Game.BasicTypesLibrary.Extensions.ConsoleExtensionMethods;
using static Game.ConsoleControlsEngine.ConsoleDrawing;
using Game.ColoredCharsEngine.Pictures;

namespace Game.ConsoleControlsEngine.Types.Pictures {
    public class ConsoleSingleCharPicture : ConsolePicture {



        public ConsoleSingleCharPicture(SingleCharPicture picture) : base(picture) {

        }



        public override void VisualizeInConsole(Point location) {
            SingleCharPicture picture = Picture as SingleCharPicture;
            string str = new string(picture.Char, picture.Width);
            CursorTop = location.Y;
            for (int i = 0; i < picture.Height; i++) {
                CursorLeft = location.X;
                CursorTop += i;
                ConsoleDrawing.Write(str);
            }
        }
    }
}
