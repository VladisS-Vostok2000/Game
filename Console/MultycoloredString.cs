using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console {
    /// <summary>
    /// Окрашенный в различные цвета текст.
    /// </summary>
    public sealed class MultycoloredString : IEnumerable<ColoredString> {
        List<ColoredString> ColoredStrings { get; } = new List<ColoredString>();
        public int Length { get; internal set; }



        public ConsolePixel this[int index] {
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



        public void Append(ColoredString coloredString) => ColoredStrings.Add(coloredString);
        public void Append(MultycoloredString multycoloredString) => ColoredStrings.AddRange(multycoloredString.ColoredStrings);
        public void RemoveAt(int index) => ColoredStrings.RemoveAt(index);


        public IEnumerator<ColoredString> GetEnumerator() => ((IEnumerable<ColoredString>)ColoredStrings).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)ColoredStrings).GetEnumerator();
        internal void PadRight(int width) => throw new NotImplementedException();
    }
}
