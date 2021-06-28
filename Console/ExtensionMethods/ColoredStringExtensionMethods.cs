using Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Console.ExtensionMethods {
    public static class ColoredStringExtensionMethods {
        /// <summary>
        /// Возвращает проходящий по строкам перечислитель заданного текста.
        /// </summary>
        public static IEnumerable<MultycoloredString> SplitToLines(this MultycoloredString value) {
            if (value == null) { throw new ArgumentNullException($"{nameof(value)} был null."); }

            var outColoredText = new MultycoloredString();
            foreach (var coloredString in value) {
                int newLineIndex = coloredString.IndexOfNewLine();
                if (newLineIndex == -1) {
                    outColoredText.Append(coloredString);
                    continue;
                }

                int startIndex = 0;
                while (newLineIndex != -1) {
                    var coloredSubstring = coloredString.ColoredSubstring(startIndex, newLineIndex - startIndex);
                    outColoredText.Append(coloredSubstring);
                    yield return outColoredText;
                    outColoredText = new MultycoloredString();
                    startIndex = newLineIndex + 1;
                    if (startIndex > coloredString.Length) { break; }

                    newLineIndex = coloredString.IndexOfNewLine(startIndex);
                }
                
            }

            yield return outColoredText;
        }

    }
}
