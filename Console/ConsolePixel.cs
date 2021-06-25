using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console {
    public struct ConsolePixel {
        public char Char { get; set; }
        public ConsoleColor Color { get; set; }



        public ConsolePixel(char chr, ConsoleColor color) {
            Char = chr;
            Color = color;
        }

    }
}
