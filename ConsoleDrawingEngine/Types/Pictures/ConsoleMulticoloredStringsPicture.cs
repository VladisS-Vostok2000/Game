using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine;
using static System.Console;
using static Game.ConsoleDrawingEngine.ConsoleDrawing;
using static Game.BasicTypesLibrary.Extensions.ConsoleExtensionMethods;

namespace Game.ConsoleDrawingEngine.Types {
    public class ConsoleMulticoloredStringsPicture : ConsolePicture {



        public ConsoleMulticoloredStringsPicture(MulticoloredStringsPicture picture) : base(picture) {

        }



        public override void VisualizeInConsole(Point location) {
            CursorPosition = location;
            int i = 0;
            foreach (var multicoloredString in ((MulticoloredStringsPicture)Picture).ToMulticoloredStrings()) {
                WriteColored(multicoloredString);
                LineDown(location.X);
            }
        }

    }
}
