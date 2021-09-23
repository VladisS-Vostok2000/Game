using Game;
using Game.BasicTypesLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Game.BasicTypesLibrary.Extensions.BasicTypesExtensions;

namespace Game.ColoredCharsEngine {
    public class ColoredCharsPicture : Picture {
        private ColoredChar[,] picture;



        public ColoredCharsPicture(ColoredChar[,] picture) : base(GetSize(picture)) {
            if (picture is null) {
                throw new ArgumentNullException(nameof(picture));
            }
            if (picture.IsEmptyOrFlat()) {
                throw new ArgumentException("Массив обязан быть ненулевым.", nameof(picture));
            }

            this.picture = picture;
        }


        /// <summary>
        /// Возвращает <see cref="ColoredChar"/> в декартовых координатах.
        /// </summary>
        public ColoredChar this[int x, int y] {
            get {
                if (x >= Width) { throw new ArgumentOutOfRangeException(nameof(x)); }
                if (y >= Height) { throw new ArgumentOutOfRangeException(nameof(y)); }

                return picture[y, x];
            }
        }



        public static Size GetSize(ColoredChar[,] chars) {
            if (chars is null) {
                throw new ArgumentNullException(nameof(chars));
            }

            if (chars.IsEmptyOrFlat()) {
                throw new ArgumentException("Массив должен иметь корректные размеры.", nameof(chars));
            }

            return new Size(chars.GetUpperBound(1) + 1, chars.GetUpperBound(0) + 1);
        }

    }
}
