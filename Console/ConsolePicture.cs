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



        public ConsolePicture(int width, int height) {
            Width = width;
            Height = height;
            Pixels = new ConsolePixel[Width, Height];
        }



        public ConsolePixel this[int x, int y] {
            get => Pixels[x, y];
            set => Pixels[x, y] = value;
        }

    }
}
