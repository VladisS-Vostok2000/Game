using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine {
    public static class ColoredStringExtensionMethods {
        /// <summary>
        /// Возвращает проходящий по строкам перечислитель заданного текста.
        /// </summary>
        public static IEnumerable<MulticoloredStringBuilder> SplitToLines(this MulticoloredStringBuilder value) {
            if (value == null) { throw new ArgumentNullException($"{nameof(value)} был null."); }

            var outColoredText = new MulticoloredStringBuilder();
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
                    outColoredText = new MulticoloredStringBuilder();
                    startIndex = newLineIndex + 1;
                    if (startIndex > coloredString.Length) { break; }

                    newLineIndex = coloredString.IndexOfNewLine(startIndex);
                }
                
            }

            yield return outColoredText;
        }

    }
}
