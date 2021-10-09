using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Game.ColoredCharsEngine;
using Game.ConsoleControlsEngine;
using static System.Console;
using static Game.BasicTypesLibrary.ConsoleExtensionMethods;
using static Game.ConsoleControlsEngine.ConsoleDrawing;

namespace Game.ConsoleControlsEngine {
    public class ConsoleColoredCharsPictureControl : ConsoleControl {
        ColoredCharsPicture Picture { get; }
        public override Size Size { get; }



        /// <summary>
        /// Создаст консольный контрол размером заданного изображения.
        /// </summary>
        public ConsoleColoredCharsPictureControl(ColoredCharsPicture picture, Point location = default) : base(location) {
            Picture = picture;
            Size = picture.Size;
        }



        public override void VisualizeInConsole() {
            CursorPosition = Location;
            int width = Math.Min(LineFreeSpace, Picture.Width);
            int height = Math.Min(FreeLines, Picture.Height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    WriteColored(Picture[x, y]);
                }
                CursorLeft = Location.X;
                CursorTop++;
            }
        }

    }
}
