using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsolePicture {
        public int Width { get; }
        public int Height { get; }
        private MulticoloredString[] Picture { get; }



        // TODO:
        public ConsolePicture(MulticoloredString[] consolePixel) {

        }
        public ConsolePicture(ColoredChar[,] consolePixel) {
            
        }



        // TODO:
        public IEnumerable<ColoredString> ToStrings() { }

    }
}
