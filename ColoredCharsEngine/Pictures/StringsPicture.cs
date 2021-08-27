using Game.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ColoredCharsEngine.Types.Pictures {
    public sealed class StringsPicture : Picture {
        private readonly string[] picture;



        public StringsPicture(string[] picture) : base(GetSize(picture)) {
            this.picture = picture;
        }



        public static Size GetSize(string[] strings) {
            if (strings is null) {
                throw new ArgumentNullException(nameof(strings));
            }
            if (strings.Empty()) {
                throw new ArgumentException("Массив пуст.", nameof(strings));
            }

            int width = strings[0].Length;
            foreach (var str in strings) {
                if (str.Length != width) {
                    throw new ArgumentException("Строки не составляют квадрат", nameof(strings));
                }
            }

           return new Size(width, strings.Length);
        }
        public IEnumerable<string> ToStrings() {
            int i = 0;
            foreach (var str in picture) {
                yield return picture[i++];
            }
        }

    }
}
