using Game.ColoredCharsEngine.Types.Pictures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ConsoleControlsEngine.ConsoleDrawing;
using static Game.BasicTypesLibrary.Extensions.BasicTypesExtensions;
using static Game.BasicTypesLibrary.Extensions.ConsoleExtensionMethods;
using static System.Console;

namespace Game.ConsoleControlsEngine.Types.Pictures {
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
