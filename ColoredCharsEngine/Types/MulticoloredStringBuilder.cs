﻿using Game.BasicTypesLibrary.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ColoredCharsEngine {
    /// <summary>
    /// Окрашенный в различные цвета текст.
    /// </summary>
    public sealed class MulticoloredStringBuilder : IEnumerable<ColoredString> {
        private List<ColoredString> ColoredStrings { get; }
        public int Length {
            get {
                int length = 0;
                foreach (var str in ColoredStrings) {
                    length += str.Length;
                }
                return length;
            }
        }



        public ColoredChar this[int index] {
            get {
                int lineIndex = 0;
                int globalIndex = 0;
                while (index > ColoredStrings[lineIndex].Length + globalIndex - 1) {
                    globalIndex += ColoredStrings[lineIndex].Length;
                    lineIndex++;
                    if (lineIndex > ColoredStrings.Count) {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }

                return ColoredStrings[lineIndex][index - globalIndex];
            }
        }



        /// <summary>
        /// Создаст новый экземпляр класса.
        /// Не принимает символ перевода строки.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public MulticoloredStringBuilder() { }
        /// <summary>
        /// Создаст новый экземпляр класса.
        /// Не принимает символ перевода строки.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public MulticoloredStringBuilder(string str) {
            ColoredStrings = new List<ColoredString>();

            ColoredStrings.Add(new ColoredString(str));
        }
        /// <summary>
        /// Создаст новый экземпляр класса.
        /// Не принимает символ перевода строки.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public MulticoloredStringBuilder(ColoredString coloredString) {
            ColoredStrings = new List<ColoredString>();

            ColoredStrings.Add(coloredString);
        }
        /// <summary>
        /// Создаст новый экземпляр класса.
        /// Не принимает символ перевода строки.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public MulticoloredStringBuilder(params IEnumerable<ColoredString>[] coloredStringsArray) {
            ColoredStrings = new List<ColoredString>();

            foreach (var coloredStrings in coloredStringsArray) {
                foreach (var coloredString in coloredStrings) {
                    ColoredStrings.Add(coloredString);
                }
            }
        }


        public static implicit operator MulticoloredStringBuilder(string v) => new MulticoloredStringBuilder(new ColoredString(v));
        public static MulticoloredStringBuilder operator +(MulticoloredStringBuilder v1, MulticoloredStringBuilder v2) {
            var outMulticoloredString = new MulticoloredStringBuilder();
            outMulticoloredString.Add(v1);
            outMulticoloredString.Add(v2);
            return outMulticoloredString;
        }
        public static MulticoloredStringBuilder operator +(MulticoloredStringBuilder v1, ColoredString v2) {
            v1.Add(v2);
            return v1;
        }



        public IEnumerator<ColoredString> GetEnumerator() => ((IEnumerable<ColoredString>)ColoredStrings).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)ColoredStrings).GetEnumerator();


        // TASK: добавить Add(string str).
        ///
        /// <returns> this. </returns>
        public MulticoloredStringBuilder Add(ColoredString coloredString) {
            ColoredStrings.Add(coloredString);
            return this;
        }
        ///
        /// <returns> this. </returns>
        public MulticoloredStringBuilder Add(MulticoloredStringBuilder multicoloredString) {
            ColoredStrings.AddRange(multicoloredString.ColoredStrings);
            return this;
        }
        ///
        /// <returns> this. </returns>
        public MulticoloredStringBuilder RemoveAt(int index) {
            ColoredStrings.RemoveAt(index);
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


        public void PadRight(int width) {
            int length = 0;
            foreach (var coloredString in ColoredStrings) {
                length += coloredString.Length;
            }

        }


        public override string ToString() {
            var sb = new StringBuilder();
            foreach (var coloredString in ColoredStrings) {
                sb.Append(coloredString.Text);
            }
            return sb.ToString();
        }
        public MulticoloredString ToMulticoloredString() => new MulticoloredString(ColoredStrings);

    }
}
