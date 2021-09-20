using Game.BasicTypesLibrary.ExtensionMethods;
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



        public ColoredString this[int index] {
            get {
                if (coloredStrings.Count() <= index) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return coloredStrings[index];
            }
        }



        public MulticoloredString(IEnumerable<ColoredString> coloredStrings) {
            this.coloredStrings = new List<ColoredString>();
            foreach (var str in coloredStrings) {
                this.coloredStrings.Add(str);
                Length += str.Length;
            }

            if (this.coloredStrings.Count == 0) {
                Empty = true;
            }
            Empty = false;
        }
        public MulticoloredString(ColoredString cs) {
            this.coloredStrings = new List<ColoredString>();
            coloredStrings.Add(cs);
            Length = cs.Length;
        }
        /// <summary>
        /// Создаст экземпляр класса из единственной строки, переведённой в <see cref="ColoredString"></see>/>.
        /// </summary>
        public MulticoloredString(string str) : this(str.ToColoredString()) {
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
        /// Вернёт новый экземпляр <see cref="MulticoloredString"/>, чья длинна, с учётом добавленных справа пробелов, будет увеличена по заданное число.
        /// </summary>
        /// <remarks> Новый экземпляр не будет создан при большей длинне строки. </remarks>
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



        public static MulticoloredString operator +(MulticoloredString v1, ColoredString v2) {
            return new MulticoloredStringBuilder(v1).Append(v2).ToMulticoloredString();
        }
        public static MulticoloredString operator +(MulticoloredString v1, string v2) {
            return v1 + v2.ToColoredString();
        }
        public static MulticoloredString operator +(MulticoloredString v1, MulticoloredString v2) {
            return new MulticoloredStringBuilder(v1, v2).ToMulticoloredString();
        }

    }
}
