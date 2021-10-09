using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.BasicTypesLibrary;

namespace Game.ColoredCharsEngine {
    /// <summary>
    /// Окрашенный в различные цвета текст.
    /// </summary>
    public sealed class MulticoloredStringBuilder : IEnumerable<ColoredString> {
        private List<ColoredString> coloredStrings;
        public IReadOnlyCollection<ColoredString> ColoredStrings;
        // REFACTORING: сделать length полем, потому что легко контроллируется.
        public int Length {
            get {
                int length = 0;
                foreach (var str in coloredStrings) {
                    length += str.Length;
                }
                return length;
            }
        }
        public bool Empty => coloredStrings.Count == 0;



        public MulticoloredStringBuilder() {
            coloredStrings = new List<ColoredString>();
        }
        public MulticoloredStringBuilder(params ColoredString[] coloredStrings) {
            this.coloredStrings = coloredStrings.ToList();
        }
        public MulticoloredStringBuilder(params MulticoloredString[] multicoloredStringsArray) {
            coloredStrings = new List<ColoredString>();
            foreach (var multicoloredString in multicoloredStringsArray) {
                coloredStrings.AddRange(multicoloredString.ColoredStrings);
            }
        }
        public MulticoloredStringBuilder(params IEnumerable<ColoredString>[] coloredStringsEnumerables) {
            coloredStrings = new List<ColoredString>();

            foreach (var coloredStrings in coloredStringsEnumerables) {
                this.coloredStrings.AddRange(coloredStrings);
            }
        }



        public ColoredChar this[int index] {
            get {
                int lineIndex = 0;
                int globalIndex = 0;
                while (index > coloredStrings[lineIndex].Length + globalIndex - 1) {
                    globalIndex += coloredStrings[lineIndex].Length;
                    lineIndex++;
                    if (lineIndex > coloredStrings.Count) {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }

                return coloredStrings[lineIndex][index - globalIndex];
            }
        }



        public static MulticoloredStringBuilder operator +(ColoredString v1, MulticoloredStringBuilder v2) {
            return v2.Prepend(v1);
        }
        public static MulticoloredStringBuilder operator +(MulticoloredStringBuilder v1, ColoredString v2) {
            return v1.Append(v2);
        }
        public static MulticoloredStringBuilder operator +(MulticoloredString v1, MulticoloredStringBuilder v2) {
            return v2.PrependRange(v1.ColoredStrings);
        }
        public static MulticoloredStringBuilder operator +(MulticoloredStringBuilder v1, MulticoloredString v2) {
            return v1.AppendRange(v2.ColoredStrings);
        }



        public IEnumerator<ColoredString> GetEnumerator() => ((IEnumerable<ColoredString>)coloredStrings).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)coloredStrings).GetEnumerator();


        /// <summary>
        /// Добавит заданныe <see cref="ColoredString"/> в конец <see cref="MulticoloredStringBuilder"/>.
        /// </summary>
        /// <returns> Ссылка на текущий экземпляр. </returns>
        public MulticoloredStringBuilder Append(params ColoredString[] coloredStrings) {
            this.coloredStrings.AddRange(coloredStrings);
            return this;
        }
        /// <summary>
        /// Добавит заданныe <see cref="IEnumerable{ColoredString}"/> в конец <see cref="MulticoloredStringBuilder"/>.
        /// </summary>
        /// <returns> Ссылка на текущий экземпляр. </returns>
        public MulticoloredStringBuilder AppendRange(IEnumerable<ColoredString> coloredStringEnums) {
            coloredStrings.AddRange(coloredStringEnums);
            return this;
        }
        /// <summary>
        /// Добавит заданный <see cref="ColoredString"/> в начало <see cref="MulticoloredStringBuilder"/>.
        /// </summary>
        /// <returns> Ссылка на текущий экземпляр. </returns>
        public MulticoloredStringBuilder Prepend(params ColoredString[] coloredStrings) {
            this.coloredStrings.PrependRange(coloredStrings);
            return this;
        }
        /// <summary>
        /// Добавит заданный <see cref="IEnumerable{ColoredString}"/> в начало <see cref="MulticoloredStringBuilder"/>.
        /// </summary>
        /// <returns> Ссылка на текущий экземпляр. </returns>
        public MulticoloredStringBuilder PrependRange(IEnumerable<ColoredString> coloredStringsEnumerables) {
            coloredStringsEnumerables.PrependRange(coloredStringsEnumerables);
            return this;
        }

        /// <summary>
        /// Удалит <see cref="ColoredString"/> с заданным индексом.
        /// </summary>
        /// <returns> Ссылка на текущий экземпляр. </returns>
        public MulticoloredStringBuilder RemoveAt(int index) {
            // TASK: AOORException.
            coloredStrings.RemoveAt(index);
            return this;
        }

        ///// <summary>
        ///// Возвращает проходящий по строкам перечислитель заданного текста.
        ///// </summary>
        //public IEnumerable<MulticoloredStringBuilder> SplitToLines() {
        //    var outColoredText = new MulticoloredStringBuilder();
        //    foreach (var coloredString in ColoredStrings) {
        //        int newLineIndex = coloredString.IndexOfNewLine();
        //        if (newLineIndex == -1) {
        //            outColoredText.Append(coloredString);
        //            continue;
        //        }

        //        int startIndex = 0;
        //        while (newLineIndex != -1) {
        //            var coloredSubstring = coloredString.ColoredSubstring(startIndex, newLineIndex - startIndex);
        //            outColoredText.Append(coloredSubstring);
        //            yield return outColoredText;
        //            outColoredText = new MulticoloredStringBuilder();
        //            startIndex = newLineIndex + 1;
        //            if (startIndex > coloredString.Length) { break; }

        //            newLineIndex = coloredString.IndexOfNewLine(startIndex);
        //        }

        //    }

        //    yield return outColoredText;
        //}


        /// <summary>
        /// Добавляет к последней строке столько пробелов, чтобы общая длинна строки была
        /// не ниже заданной.
        /// </summary>
        public void PadRight(int width) {
            if (Empty) {
                coloredStrings.Add(new ColoredString(' ', width));
                return;
            }
            if (Length >= width) {
                return;
            }

            int diff = width - Length;
            ColoredString lastItem = coloredStrings.Last();
            coloredStrings[coloredStrings.Count - 1] = lastItem + new string(' ', diff);
        }


        public override string ToString() {
            var sb = new StringBuilder();
            foreach (var coloredString in coloredStrings) {
                sb.Append(coloredString.Text);
            }
            return sb.ToString();
        }
        public MulticoloredString ToMulticoloredString() => new MulticoloredString(coloredStrings);

    }
}
