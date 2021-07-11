using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ConsoleEngine {
    public sealed class ColoredCharTextbox : IConsoleDrawable {
        public Point Location {
            get => new Point(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }
        public int X { get; set; }
        public int Y { get; set; }


        public int Width { get; }
        public int Height { get; }


        
        public Picture ConsolePicture { get; }



        public ColoredCharTextbox(MulticoloredString text, int width, int height) {
            if (width < 0) {
                throw new TextBoxInvalidArgumentException($"Ширина должна быть больше нуля. {nameof(width)} был {width}.", width);
            }
            if (height < 0) {
                throw new TextBoxInvalidArgumentException($"Высота должна быть больше нуля. {nameof(height)} был {height}.", height);
            }

            ConsolePicture = Render(text);
        }



        // TODO: должен возвращать значение, чтобы при изменении кода не нужно
        // было лопатить весь метод.
        private Picture Render(MulticoloredString text) {
            ColoredCharsPicture outPicture;
            MulticoloredString[] arrangedText = new MulticoloredString[Height];
            int lineIndex = 0;
            WriteMultycoloredText();
            PadArrangedText();
            return new MulticoloredStringsPicture(arrangedText);

            void WriteMultycoloredText() {
                foreach (var multycoloredLine in text.SplitToLines()) {
                    WriteMultycoloredLine(multycoloredLine);
                    if (++lineIndex >= Height) { break; }
                }
            }

            void WriteMultycoloredLine(MulticoloredString multycoloredLine) {
                if (multycoloredLine.Length <= ArrangedTextCurrentLineFreeSpace()) {
                    arrangedText[lineIndex].Append(multycoloredLine);
                    return;
                }

                foreach (var coloredString in multycoloredLine) {
                    WriteColoredString(coloredString);
                }
            }

            int ArrangedTextCurrentLineFreeSpace() => ArrangedTextLineFreeSpace(lineIndex);

            int ArrangedTextLineFreeSpace(int index) => Width - arrangedText[index].Length;

            void WriteColoredString(ColoredString coloredString) {
                bool isSeparator = char.IsWhiteSpace(coloredString[0].Char);
                foreach (var charSequence in ReadSequence(coloredString)) {
                    WriteSequence(charSequence, isSeparator);
                    if (lineIndex >= Height) { return; }

                    isSeparator = !isSeparator;
                }
            }

            IEnumerable<ColoredString> ReadSequence(ColoredString value) {
                int i = 0;
                while (i < value.Length) {
                    var sb = new StringBuilder(value[i].Char);
                    bool startType = char.IsWhiteSpace(value[i].Char);
                    for (i++; i < value.Length; i++) {
                        char letter = value[i].Char;
                        bool letterType = char.IsWhiteSpace(letter);
                        if (letterType != startType) { break; }

                        sb.Append(letter);
                    }

                    yield return new ColoredString(sb.ToString(), value.Color);
                }
            }

            void WriteSequence(ColoredString sequence, bool isSeparator) {
                if (sequence.Length < ArrangedTextCurrentLineFreeSpace()) {
                    arrangedText[lineIndex].Append(sequence);
                    return;
                }

                if (isSeparator) {
                    WriteSeparator(sequence);
                    return;
                }

                if (sequence.Length > Width) {
                    WriteSolidSequence(sequence);
                    return;
                }

                lineIndex++;
                if (lineIndex >= Height) { return; }
                arrangedText[lineIndex].Append(sequence.ColoredSubstring(lineIndex));
            }

            void WriteSeparator(ColoredString sequence) {
                arrangedText[lineIndex].Append(sequence.ColoredSubstring(0, ArrangedTextCurrentLineFreeSpace()));
                // Один пробел удаляется при переносе строки, чтобы избежать "красных строк".
                int sequenceStartIndex = ArrangedTextCurrentLineFreeSpace() + 1;
                while (lineIndex < Height && sequenceStartIndex < sequence.Length) {
                    FitSequence(sequence, sequenceStartIndex);
                    lineIndex++;
                    sequenceStartIndex += Width + 1;
                }
            }

            void WriteSolidSequence(ColoredString sequence) {
                arrangedText[lineIndex].Append(sequence.ColoredSubstring(0, ArrangedTextCurrentLineFreeSpace()));
                int sequenceIndex = ArrangedTextCurrentLineFreeSpace();
                while (lineIndex++ < Height && sequenceIndex < sequence.Length) {
                    FitSequence(sequence, sequenceIndex);
                    lineIndex++;
                    sequenceIndex += Width;
                }
            }

            void FitSequence(ColoredString sequence, int sequenceStartIndex) {
                if (sequenceStartIndex - sequence.Length < Width) {
                    arrangedText[lineIndex].Append(sequence.ColoredSubstring(sequenceStartIndex));
                    return;
                }
                else {
                    arrangedText[lineIndex].Append(sequence.ColoredSubstring(sequenceStartIndex, Width));
                }
            }

            void PadArrangedText() {
                foreach (var multycoloredString in arrangedText) {
                    multycoloredString.PadRight(Width);
                }
            }

        }

    }
}
