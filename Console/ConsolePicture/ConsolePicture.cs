﻿using Game.Console.ConsolePicture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Console {
    public sealed class ConsolePicture {
        public int Width { get; }
        public int Height { get; }
        public ConsolePixel[,] Pixels { get; }



        public ConsolePicture(int width, int height, ConsolePixel[,] consolePixel) {
            if (width < 0) {
                throw new ConsolePictureInvalidArgumentException($"Ширина должна быть больше нуля. {nameof(width)} был {width}.", width);
            }
            if (height < 0) {
                throw new ConsolePictureInvalidArgumentException($"Высота должна быть больше нуля. {nameof(height)} был {height}.", height);
            }
            Width = width;
            Height = height;
            Pixels = consolePixel;
        }



        public ConsolePixel this[int x, int y] => Pixels[x, y];

    }
}
