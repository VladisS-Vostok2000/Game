using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Console.TextBox {
    public class TextBox : IConsoleDrawable {
        private StringBuilder[] Text { get; }
        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConsolePicture ConsolePicture => throw new NotImplementedException();

        public TextBox(string text, int width, int height) {
            Width = width > 0 ? width : throw new TextBoxInvalidArgumentException($"Ширина обязана быть больше нуля. {nameof(width)} была {width}", width);
            Height = height > 0 ? height : throw new TextBoxInvalidArgumentException($"Высота обязана быть больше нуля. {nameof(height)} была {height}", width);
            Text = new StringBuilder[Height].Fill(() => new StringBuilder()) ?? throw new TextBoxInvalidArgumentException($"{nameof(text)} был null.", text);
            CharImage.Fill(() => string.Empty);
            Render(text);
        }



        private void Render(string text) {
            int lineIndex = 0;
            foreach (string line in text.SplitToLines()) {
                WriteLine(line, ref lineIndex);
                lineIndex++;
                if (lineIndex >= Height) { break; }
            }
            for (int i = 0; i < text.Length; i++) {
                char str = text[i];
                CharImage[i] = str.ToString().PadRight(Width);
            }


            void WriteLine(string line, ref int _lineIndex) {
                if (line.Length <= LineFreeSpace(_lineIndex)) {
                    Text[_lineIndex].Append(line);
                    return;
                }

                bool isSeparator = char.IsWhiteSpace(line[0]);
                foreach (var charSequence in Read(line)) {
                    WriteSequense(charSequence, ref _lineIndex, isSeparator);
                    if (_lineIndex >= Height) { return; }
                    isSeparator = !isSeparator;
                }
            }

            void WriteSequense(string sequence, ref int _lineIndex, bool isSeparator) {
                if (sequence.Length < LineFreeSpace(_lineIndex)) {
                    Text[_lineIndex].Append(sequence);
                    return;
                }

                if (isSeparator || sequence.Length > Width) {
                    WriteLongSequence(sequence, ref _lineIndex);
                    return;
                }

                _lineIndex++;
                if (_lineIndex >= Height) {
                    return;
                }

                Text[_lineIndex].Append(sequence);
            }

            void WriteLongSequence(string _sequence, ref int _lineIndex) {
                int sequenceIndex = LineFreeSpace(_lineIndex);
                Text[_lineIndex].Append(_sequence.Substring(0, sequenceIndex));

                while (sequenceIndex < _sequence.Length && ++_lineIndex < Height) {
                    if (sequenceIndex - _sequence.Length < Width) {
                        Text[_lineIndex].Append(_sequence.Substring(sequenceIndex));
                        break;
                    }
                    else {
                        Text[_lineIndex].Append(_sequence.Substring(sequenceIndex, Width));
                        sequenceIndex += Width;
                    }
                }
            }
        }
        private static IEnumerable<string> Read(string sourse) {
            int i = 0;
            while (i < sourse.Length) {
                var sb = new StringBuilder(sourse[i]);
                bool startType = char.IsWhiteSpace(sourse[i]);
                for (i++; i < sourse.Length; i++) {
                    char letter = sourse[i];
                    bool letterType = char.IsWhiteSpace(letter);
                    if (letterType != startType) { break; }

                    sb.Append(letter);
                }

                yield return sb.ToString();
            }
        }
        private int LineFreeSpace(int index) => Width - Text[index].Length;

    }
}
