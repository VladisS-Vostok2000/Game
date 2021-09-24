using Game.BasicTypesLibrary.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ColoredCharsEngine {
    /// <summary>
    /// Строка, содержащая строки разных цветов.
    /// </summary>
    public sealed class MulticoloredString : IEnumerable<ColoredString> {
        private List<ColoredString> coloredStrings;
        public IReadOnlyList<ColoredString> ColoredStrings => coloredStrings.AsReadOnly();


        public readonly int Length;
        public readonly bool Empty;



        public MulticoloredString(params ColoredString[] coloredStrings) {
            this.coloredStrings = new List<ColoredString>();
            this.coloredStrings.AddRange(coloredStrings);

            Length = 0;
            Empty = this.coloredStrings.Empty();
            if (Empty) {
                return;
            }

            foreach (var coloredString in coloredStrings) {
                Length += coloredString.Length;
            }
        }
        public MulticoloredString(params IEnumerable<ColoredString>[] coloredStringsEnums) {
            coloredStrings = new List<ColoredString>();
            foreach (var coloredStringsEnum in coloredStringsEnums) {
                coloredStrings.AddRange(coloredStringsEnum);
            }

            Length = 0;
            Empty = coloredStrings.Count == 0;
            if (Empty) {
                return;
            }

            foreach (var coloredString in coloredStrings) {
                Length += coloredString.Length;
            }
        }



        public ColoredString this[int index] {
            get {
                if (coloredStrings.Count() <= index) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return coloredStrings[index];
            }
        }




        // TASK: Append и prepend.
        public static explicit operator MulticoloredString(ColoredString cs) {
            return new MulticoloredString(cs);
        }
        public static MulticoloredString operator +(MulticoloredString v1, ColoredString v2) {
            return new MulticoloredString(v1.coloredStrings.Append(v2));
        }
        public static MulticoloredString operator +(MulticoloredString v1, MulticoloredString v2) {
            return new MulticoloredString(v1.ColoredStrings, v2.ColoredStrings);
        }
        public static MulticoloredString operator +(ColoredString v1, MulticoloredString v2) {
            var coloredStrings = new List<ColoredString>(v2.coloredStrings);
            coloredStrings.AddHead(v1);
            return new MulticoloredString(coloredStrings);
        }



        public IEnumerator<ColoredString> GetEnumerator() {
            return coloredStrings.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return coloredStrings.GetEnumerator();
        }


        public override string ToString() {
            var sb = new StringBuilder();
            foreach (var cs in coloredStrings) {
                sb.Append(cs);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Вернёт новый экземпляр <see cref="MulticoloredString"/>, чья длинна,
        /// с учётом добавленных справа пробелов, будет увеличена по заданное число.
        /// </summary>
        /// <remarks> Будет возвращён текущий <see cref="MulticoloredString"/>
        /// при достаточной длинне строки. </remarks>
        public MulticoloredString PadRight(int length) {
            if (Length >= length) {
                return this;
            }

            int dif = length - Length;
            var outStrings = new List<ColoredString>();
            outStrings.AddRange(coloredStrings);
            outStrings.Add(new ColoredString(' ', dif));
            return new MulticoloredString(outStrings);
        }

    }
}
