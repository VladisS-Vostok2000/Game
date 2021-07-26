using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.ColoredCharsEngine {
    public class ColoredCharsPicture : Picture {
        private ColoredChar[,] picture;



        public ColoredCharsPicture(ColoredChar[,] picture) {
            this.picture = picture;
        }



        public ColoredChar this[int x, int y] {
            get {
                if (x >= Width) throw new ArgumentOutOfRangeException(nameof(x));
                if (y >= Height) throw new ArgumentOutOfRangeException(nameof(y));
                return picture[x, y];
            }
        }

    }
}
