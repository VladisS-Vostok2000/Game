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



        public ColoredCharsPicture(ColoredChar[,] picture) : base(GetSize(picture)) {
            if (picture.IsEmptyOrFlat()) {
                throw new ArgumentException("Картинка обязана быть ненулевой.", nameof(picture));
            }

            this.picture = picture;
        }



        public ColoredChar this[int x, int y] {
            get {
                if (x >= Width) { throw new ArgumentOutOfRangeException(nameof(x)); }
                if (y >= Height) { throw new ArgumentOutOfRangeException(nameof(y)); }

                return picture[x, y];
            }
        }



        public static Size GetSize(ColoredChar[,] chars) {
            if (chars == null) {
                throw new ArgumentNullException(nameof(chars));
            }

            if (chars.IsEmptyOrFlat()) {
                throw new ArgumentException("Массив должен иметь корректные размеры.", nameof(chars));
            }

            return new Size(chars.GetUpperBound(1), chars.GetUpperBound(0));
        }

    }
}
