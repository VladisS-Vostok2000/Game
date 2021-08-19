﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ColoredCharsEngine {
    /// <summary>
    /// Окрашенный в различные цвета текст.
    /// </summary>
    // TODO: больше не принимает /r/n.
    public sealed class MulticoloredStringBuilder : IEnumerable<ColoredString> {
        private List<ColoredString> ColoredStrings { get; } = new List<ColoredString>();
        public int Length { get; internal set; }


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



        public MulticoloredStringBuilder() { }
        public MulticoloredStringBuilder(string str) {
            ColoredStrings.Add(new ColoredString(str));
        }
        public MulticoloredStringBuilder(ColoredString coloredString) {
            ColoredStrings.Add(coloredString);
        }
        public MulticoloredStringBuilder(IEnumerable<ColoredString> coloredStrings) {
            foreach (var coloredString in coloredStrings) {
                ColoredStrings.Add(coloredString);
            }
        }

        /// <returns> Объект, к которому добавили строку. </returns>
        public MulticoloredStringBuilder Append(ColoredString coloredString) {
            ColoredStrings.Add(coloredString);
            return this;
        }
        /// <returns> Объект, к которому добавили строку. </returns>
        public MulticoloredStringBuilder Append(MulticoloredStringBuilder multicoloredString) {
            ColoredStrings.AddRange(multicoloredString.ColoredStrings);
            return this;
        }
        /// <returns> Объект, у которого удалили строку. </returns>
        public MulticoloredStringBuilder RemoveAt(int index) {
            ColoredStrings.RemoveAt(index);
            return this;
        }


        public IEnumerator<ColoredString> GetEnumerator() => ((IEnumerable<ColoredString>)ColoredStrings).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)ColoredStrings).GetEnumerator();

        public void PadRight(int width) {
            int length = 0;
            foreach (var coloredString in ColoredStrings) {
                length += coloredString.Length;
            }

        }

        public static implicit operator MulticoloredStringBuilder(string v) => new MulticoloredStringBuilder(new ColoredString(v));
        public static MulticoloredStringBuilder operator +(MulticoloredStringBuilder v1, MulticoloredStringBuilder v2) {
            var outMulticoloredString = new MulticoloredStringBuilder();
            outMulticoloredString.Append(v1);
            outMulticoloredString.Append(v2);
            return outMulticoloredString;
        }
        public static MulticoloredStringBuilder operator +(MulticoloredStringBuilder v1, ColoredString v2) {
            v1.Append(v2);
            return v1;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            foreach (var coloredString in ColoredStrings) {
                sb.Append(coloredString.Text);
            }
            return sb.ToString();
        }

    }
}