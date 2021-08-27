using Game.ColoredCharsEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Game.ConsoleDrawingEngine;
using static Game.BasicTypesLibrary.ExtensionMethods.ConsoleExtensionMethods;
using static Game.ConsoleDrawingEngine.ConsoleDrawing;

namespace Game.ConsoleDrawingEngine.Types {
    public class ConsoleColoredCharsPicture : ConsolePicture {
        public ConsoleColoredCharsPicture(ColoredCharsPicture picture) : base(picture) { }



        public override void VisualizeInConsole(Point location) {
            ColoredCharsPicture s = (ColoredCharsPicture)Picture;
            CursorPosition = location;
            int width = Math.Min(LineFreeSpace, Picture.Width);
            int height = Math.Min(FreeLines, Picture.Height);
            for (int r = 0; r < height; r++) {
                for (int c = 0; c < width; c++) {
                    WriteColored(s[r, c]);
                }
                CursorLeft = location.X;
                CursorTop++;
            }
        }

    }
}
