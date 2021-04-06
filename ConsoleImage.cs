using System;

namespace Game {
    public struct ConsoleImage {
        public char Char { get; set; }
        public ConsoleColor Color { get; set; }



        public ConsoleImage(char image, ConsoleColor color) {
            Char = image;
            Color = color;
        }

    }
}
