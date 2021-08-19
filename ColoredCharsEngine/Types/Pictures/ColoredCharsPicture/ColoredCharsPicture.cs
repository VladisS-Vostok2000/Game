using Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Game.ExtensionMethods.BasicTypesExtensionsMethods;

namespace Game.ColoredCharsEngine {
    public class ColoredCharsPicture : Picture {
        private ColoredChar[,] picture;



        public ColoredCharsPicture(ColoredChar[,] picture) {
            if (picture.IsEmptyOrFlat()) {
                throw new ArgumentException("Картинка обязана быть ненулевой.", nameof(picture));
            }

            this.picture = picture;
            Size = new Size(picture.GetUpperBound(1) + 1, picture.GetUpperBound(0) + 1);
        }



        public ColoredChar this[int x, int y] {
            get {
                if (x >= Width) { throw new ArgumentOutOfRangeException(nameof(x)); }
                if (y >= Height) { throw new ArgumentOutOfRangeException(nameof(y)); }

                return picture[x, y];
            }
        }

    }
}
