using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Game.ColoredCharsEngine;
using Game.ConsoleControlsEngine;
using static System.Console;
using static Game.ConsoleControlsEngine.ConsoleDrawing;
using static Game.BasicTypesLibrary.ConsoleExtensionMethods;

namespace Game.ConsoleControlsEngine {
    public class ConsoleMulticoloredStringsPictureControl : ConsoleControl {
        public MulticoloredStringsPicture Picture { get; }
        public override Size Size { get; }



        /// <summary>
        /// Создаст консольный контрол размером заданного изображения.
        /// </summary>
        public ConsoleMulticoloredStringsPictureControl(MulticoloredStringsPicture picture, Point location = default) : base(location) {
            Picture = picture;
            Size = picture.Size;
        }



        public override void VisualizeInConsole() {
            CursorPosition = Location;
            int i = 0;
            foreach (var multicoloredString in Picture.ToMulticoloredStrings()) {
                WriteColored(multicoloredString);
                LineDown(Location.X);
            }
        }

    }
}
