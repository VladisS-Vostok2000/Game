using Game.ColoredCharsEngine.Types.Pictures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ConsoleDrawingEngine.ConsoleDrawing;
using static Game.ExtensionMethods.BasicTypesExtensionsMethods;
using static Game.ExtensionMethods.ConsoleExtensionMethods;
using static System.Console;

namespace Game.ConsoleDrawingEngine.Types.Pictures {
    class ConsoleStringsPicture : ConsolePicture {



        public ConsoleStringsPicture(StringsPicture picture) : base(picture) {

        }



        public override void VisualizeInConsole(Point location) {
            int i = 0;
            foreach (var str in ((StringsPicture)Picture).ToStrings()) {
                CursorLeft = location.X;
                CursorTop = location.Y + i;
                ConsoleDrawing.Write(str);
            }
        }

    }
}
