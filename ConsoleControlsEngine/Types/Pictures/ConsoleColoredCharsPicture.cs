using Game.ColoredCharsEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Game.ConsoleControlsEngine;
using static Game.BasicTypesLibrary.Extensions.ConsoleExtensionMethods;
using static Game.ConsoleControlsEngine.ConsoleDrawing;

namespace Game.ConsoleControlsEngine.Types {
    public class ConsoleColoredCharsPicture : ConsolePicture {
        public ConsoleColoredCharsPicture(ColoredCharsPicture picture) : base(picture) {

        }



        public override void VisualizeInConsole(Point location) {
            ColoredCharsPicture picture = (ColoredCharsPicture)Picture;
            CursorPosition = location;
            int width = Math.Min(LineFreeSpace, Picture.Width);
            int height = Math.Min(FreeLines, Picture.Height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    WriteColored(picture[x, y]);
                }
                CursorLeft = location.X;
                CursorTop++;
            }
        }

    }
}
