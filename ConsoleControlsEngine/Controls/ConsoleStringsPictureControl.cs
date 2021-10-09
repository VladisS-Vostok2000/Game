using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Game.ConsoleControlsEngine;
using Game.ColoredCharsEngine;
using static Game.ConsoleControlsEngine.ConsoleDrawing;
using static Game.BasicTypesLibrary.BasicTypesExtensions;
using static Game.BasicTypesLibrary.ConsoleExtensionMethods;
using static System.Console;

namespace Game.ConsoleControlsEngine {
    class ConsoleStringsPictureControl : ConsoleControl {
        public StringsPicture Picture { get; }
        public override Size Size { get; }


        /// <summary>
        /// Создаст консольный контрол размером заданного изображения.
        /// </summary>
        public ConsoleStringsPictureControl(StringsPicture picture, Point location = default) : base(location) {
            Picture = picture;
            Size = picture.Size;
        }



        public override void VisualizeInConsole() {
            int i = 0;
            foreach (var str in Picture.ToStrings()) {
                CursorLeft = Location.X;
                CursorTop = Location.Y + i;
                ConsoleDrawing.Write(str);
            }
        }

    }
}
