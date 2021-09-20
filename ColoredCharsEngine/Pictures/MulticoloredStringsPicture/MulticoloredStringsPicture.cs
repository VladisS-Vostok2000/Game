using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.BasicTypesLibrary.ExtensionMethods.BasicTypesExtensionsMethods;
using static Game.ColoredCharsEngine.StaticMethods.GraphicsModificate;
using Game.BasicTypesLibrary.ExtensionMethods;
using Game.ColoredCharsEngine.Types;

namespace Game.ColoredCharsEngine {
    public class MulticoloredStringsPicture : Picture {
        private MulticoloredString[] picture;



        public MulticoloredStringsPicture(MulticoloredString[] picture) : base(GetSize(picture)) {
            this.picture = picture;
        }



        public IEnumerable<MulticoloredString> ToMulticoloredStrings() {
            for (int i = 0; i < picture.Length; i++) {
                yield return picture[i];
            }
        }


        public static Size GetSize(MulticoloredString[] strings) {
            // REFACTORNING: вынести исключения в базовый класс.
            if (strings is null) {
                throw new ArgumentNullException(nameof(strings));
            }
            if (strings.Empty()) {
                throw new ArgumentException("Изображение не может быть пустым.", nameof(picture));
            }
            if (!IsRectangular(strings)) {
                throw new ArgumentException($"Заданный аргумент обязан быть прямоугольным.", nameof(picture));
            }

            return new Size(strings[0].Length, strings.Length);
        }

    }
}
