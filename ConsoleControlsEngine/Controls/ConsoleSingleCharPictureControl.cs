using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Game.ColoredCharsEngine;
using Game.ConsoleControlsEngine;
using System.Windows.Forms;
using static System.Console;
using static Game.BasicTypesLibrary.BasicTypesExtensions;
using static Game.BasicTypesLibrary.ConsoleExtensionMethods;
using static Game.ConsoleControlsEngine.ConsoleDrawing;

namespace Game.ConsoleControlsEngine {
    public class ConsoleSingleCharPictureControl : ConsoleControl {
        public SingleCharPicture Picture { get; }
        public override Size Size { get; }



        /// <summary>
        /// Создаст консольный контрол размером заданного изображения.
        /// </summary>
        public ConsoleSingleCharPictureControl(SingleCharPicture picture, Point location = default) : base(location) {
            Picture = picture;
            Size = picture.Size;
        }



        public override void VisualizeInConsole() {
            string str = new string(Picture.Char, Picture.Width);
            CursorTop = Location.Y;
            for (int i = 0; i < Picture.Height; i++) {
                CursorLeft = Location.X;
                CursorTop += i;
                ConsoleDrawing.Write(str);
            }
        }


        public static ConsoleSingleCharPictureControl CreateVoidConsolePicture(Size size, Point location = default) {
            return new ConsoleSingleCharPictureControl(new SingleCharPicture(size, ' '), location);
        }

    }
}
