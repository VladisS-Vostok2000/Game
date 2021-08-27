using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Game.BasicTypesLibrary.ExtensionMethods;
using System.Drawing;

namespace Game.Parser {
    public static class PointListParser {
        /// <summary>
        /// Возвращает <see cref="List{T}}"/> <see cref="Point"/>.
        /// Формат предполагает "_{_{_0_;_0_}_}_".
        /// </summary>
        public static List<Point> ParsePouintList(string sourse) {
            var outList = new List<Point>();

            var stringPoints = new List<string>();
            for (int levelIndex = sourse.IndexOf('{') + 1; levelIndex < sourse.Length; levelIndex++) {
                char letter = sourse[levelIndex];
                if (letter == '{') {
                    stringPoints.Add(ExtractLevel(sourse, levelIndex));
                    levelIndex = sourse.IndexOf('}', levelIndex);
                }
            }

            foreach (var stringPoint in stringPoints) {
                outList.Add(ParsePoint(stringPoint));
            }

            return outList;
        }
        private static string ExtractLevel(string sourse, int startIndex) {
            startIndex = sourse.IndexOf('{', startIndex) + 1;
            var sb = new StringBuilder();
            for (int i = startIndex; ; i++) {
                char letter = sourse[i];
                if (letter == '}') { break; }
                sb.Append(letter);
            }
            return sb.ToString();
        }
        private static Point ParsePoint(string sourse) {
            string[] stringIntegers = sourse.Split(',');
            int[] integers = new int[2];
            for (int i = 0; i < stringIntegers.Length; i++) {
                string stringInteger = stringIntegers[i];
                integers[i] = int.Parse(stringInteger);
            }
            return new Point(integers[0], integers[1]);
        }

    }
}
