using Game.BasicTypesLibrary.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Game.ConsoleDrawingEngine.Types;
using Game.ColoredCharsEngine;
using Game.ColoredCharsEngine.Types;
using System.Runtime.InteropServices.ComTypes;
using Game.ColoredCharsEngine.StaticMethods;

namespace Game.ConsoleDrawingEngine.ConsoleControls {
    public sealed class ConsoleTextboxControl : ConsoleControl {
        private MulticoloredString[] picture;
        public override ConsolePicture ConsolePicture { get; }



        /// <summary>
        /// Создаёт экземпляр <see cref="ConsoleTextboxControl"/>.
        /// Строки будут выровнены добавочными пробелами по самую длинную строчку.
        /// </summary>
        public ConsoleTextboxControl(Point location, MulticoloredString[] text) : base(location) {
            if (text is null) {
                throw new ArgumentNullException(nameof(text));
            }
            if (!GraphicsModificate.IsRectangular(text)) {
                throw new ArgumentException("Невозможно создать экземпляр из непрямоугольного на печати массива.", nameof(text));
            }

            picture = new MulticoloredString[text.Length];
            for (int i = 0; i < text.Length; i++) {
                picture[i] = text[i];
            }

            ConsolePicture = new ConsoleMulticoloredStringsPicture(new MulticoloredStringsPicture(picture));
        }
        /// <summary>
        /// Создаёт экземпляр <see cref="ConsoleTextboxControl"/> единичной высоты.
        /// </summary>
        public ConsoleTextboxControl(Point location, MulticoloredString text) : base(location) {
            if (text is null) {
                throw new ArgumentNullException(nameof(text));
            }

            picture = new MulticoloredString[1];
            picture[0] = text;
            ConsolePicture = new ConsoleMulticoloredStringsPicture(new MulticoloredStringsPicture(picture));
        }



        //private MulticoloredStringBuilder[] Render(MulticoloredStringBuilder text) {
        //    MulticoloredStringBuilder[] arrangedText = new MulticoloredStringBuilder[Height];
        //    int lineIndex = 0;
        //    WriteMultycoloredText();
        //    PadArrangedText();
        //    return arrangedText;

        //    void WriteMultycoloredText() {
        //        foreach (var multycoloredLine in text.SplitToLines()) {
        //            WriteMultycoloredLine(multycoloredLine);
        //            if (++lineIndex >= Height) { break; }
        //        }
        //    }

        //    void WriteMultycoloredLine(MulticoloredStringBuilder multycoloredLine) {
        //        if (multycoloredLine.Length <= ArrangedTextCurrentLineFreeSpace()) {
        //            arrangedText[lineIndex].Append(multycoloredLine);
        //            return;
        //        }

        //        foreach (var coloredString in multycoloredLine) {
        //            WriteColoredString(coloredString);
        //        }
        //    }

        //    int ArrangedTextCurrentLineFreeSpace() => ArrangedTextLineFreeSpace(lineIndex);

        //    int ArrangedTextLineFreeSpace(int index) => Width - arrangedText[index].Length;

        //    void WriteColoredString(ColoredString coloredString) {
        //        bool isSeparator = char.IsWhiteSpace(coloredString[0].Char);
        //        foreach (var charSequence in ReadSequence(coloredString)) {
        //            WriteSequence(charSequence, isSeparator);
        //            if (lineIndex >= Height) { return; }

        //            isSeparator = !isSeparator;
        //        }
        //    }

        //    IEnumerable<ColoredString> ReadSequence(ColoredString value) {
        //        int i = 0;
        //        while (i < value.Length) {
        //            var sb = new StringBuilder(value[i].Char);
        //            bool startType = char.IsWhiteSpace(value[i].Char);
        //            for (i++; i < value.Length; i++) {
        //                char letter = value[i].Char;
        //                bool letterType = char.IsWhiteSpace(letter);
        //                if (letterType != startType) { break; }

        //                sb.Append(letter);
        //            }

        //            yield return new ColoredString(sb.ToString(), value.Color);
        //        }
        //    }

        //    void WriteSequence(ColoredString sequence, bool isSeparator) {
        //        if (sequence.Length < ArrangedTextCurrentLineFreeSpace()) {
        //            arrangedText[lineIndex].Append(sequence);
        //            return;
        //        }

        //        if (isSeparator) {
        //            WriteSeparator(sequence);
        //            return;
        //        }

        //        if (sequence.Length > Width) {
        //            WriteSolidSequence(sequence);
        //            return;
        //        }

        //        lineIndex++;
        //        if (lineIndex >= Height) { return; }
        //        arrangedText[lineIndex].Append(sequence.ColoredSubstring(lineIndex));
        //    }

        //    void WriteSeparator(ColoredString sequence) {
        //        arrangedText[lineIndex].Append(sequence.ColoredSubstring(0, ArrangedTextCurrentLineFreeSpace()));
        //        // Один пробел удаляется при переносе строки, чтобы избежать "красных строк".
        //        int sequenceStartIndex = ArrangedTextCurrentLineFreeSpace() + 1;
        //        while (lineIndex < Height && sequenceStartIndex < sequence.Length) {
        //            FitSequence(sequence, sequenceStartIndex);
        //            lineIndex++;
        //            sequenceStartIndex += Width + 1;
        //        }
        //    }

        //    void WriteSolidSequence(ColoredString sequence) {
        //        arrangedText[lineIndex].Append(sequence.ColoredSubstring(0, ArrangedTextCurrentLineFreeSpace()));
        //        int sequenceIndex = ArrangedTextCurrentLineFreeSpace();
        //        while (lineIndex++ < Height && sequenceIndex < sequence.Length) {
        //            FitSequence(sequence, sequenceIndex);
        //            lineIndex++;
        //            sequenceIndex += Width;
        //        }
        //    }

        //    void FitSequence(ColoredString sequence, int sequenceStartIndex) {
        //        if (sequenceStartIndex - sequence.Length < Width) {
        //            arrangedText[lineIndex].Append(sequence.ColoredSubstring(sequenceStartIndex));
        //            return;
        //        }
        //        else {
        //            arrangedText[lineIndex].Append(sequence.ColoredSubstring(sequenceStartIndex, Width));
        //        }
        //    }

        //    void PadArrangedText() {
        //        foreach (var multycoloredString in arrangedText) {
        //            multycoloredString.PadRight(Width);
        //        }
        //    }

        //}

    }
}
