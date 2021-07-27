﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine {
    public struct ColoredChar {
        public char Char { get; set; }
        public ConsoleColor Color { get; set; }



        public ColoredChar(char chr, ConsoleColor color) {
            Char = chr;
            Color = color;
        }

    }
}
