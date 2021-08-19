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



        public MulticoloredStringsPicture(MulticoloredStringBuilder[] picture) {
            if (picture.Empty()) {
                throw new ArgumentException("Изображение не может быть пустым.", nameof(picture));
            }

            if (!IsRectangular(picture)) {
                throw new ArgumentException($"Заданный аргумент обязан быть прямоугольным.", nameof(picture));
            }

            this.picture = picture;
            Size = new Size(picture[0].Length, picture.Length);
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

    }
}
