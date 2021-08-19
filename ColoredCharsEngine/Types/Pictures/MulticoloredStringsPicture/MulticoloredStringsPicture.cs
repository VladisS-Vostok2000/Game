using Game.ConsoleDrawingEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ExtensionMethods.BasicTypesExtensionsMethods;

namespace Game.ColoredCharsEngine {
    public class MulticoloredStringsPicture : Picture {
        private MulticoloredStringBuilder[] picture;



        public MulticoloredStringsPicture(MulticoloredStringBuilder[] picture) : base(GetSize(picture)) {
            this.picture = picture;
        }



        public IEnumerable<MulticoloredStringBuilder> ToMulticoloredStrings() {
            foreach (var multicoloredString in picture) {
                yield return multicoloredString;
            }
        }


        /// <summary>
        /// True, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(MulticoloredStringBuilder[] mSBs) {
            if (mSBs.Empty()) {
                return true;
            }

            int width = mSBs[0].Length;
            foreach (var mSB in mSBs) {
                if (mSB.Length != width) {
                    return false;
                }
            }

            return true;
        }
        public static Size GetSize(MulticoloredStringBuilder[] strings) {
            // REFACTORNING: вынести исключения в базовый класс.
            if (strings == null) {
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
