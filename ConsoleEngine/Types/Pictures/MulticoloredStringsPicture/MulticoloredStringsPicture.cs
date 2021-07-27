using Game.ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine {
    public class MulticoloredStringsPicture : Picture {
        private MulticoloredStringBuilder[] picture;



        public MulticoloredStringsPicture(MulticoloredStringBuilder[] picture) {
            // TODO: проверка на null.
            if (picture.Length < 1) {
                throw new MuticoloredStringsPictureArgumentException(picture, "Массив пуст.");
            }

            // TODO: проверка на "квадратность".
            this.picture = picture;
            Size = new System.Drawing.Size(picture.Length, picture[0].Length);
        }



        public IEnumerable<MulticoloredStringBuilder> ToMulticoloredStrings() {
            foreach (var multicoloredString in picture) {
                yield return multicoloredString;
            }
        }

    }
}
