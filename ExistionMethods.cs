using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    internal static class ExistionMethods {
        /// <summary>
        /// Отчистит строку от символов, относящихся к категории пробелов.
        /// </summary>
        internal static string ClearEmptySpaces(this string target) {
            string outString = "";
            for (int i = 0; i < target.Length; i++) {
                if (!char.IsWhiteSpace(target[i])) {
                    outString += target[i];
                }
            }
            return outString;
        }

        /// <summary>
        /// Гарантирует нахождение числа в заданном диапазоне.
        /// </summary>
        internal static int InRange(in this int target, in int rangeStart, in int RangeLength) {
            int rawIndex = target - rangeStart;
            int index = rawIndex % RangeLength;
            return index < 0 ? index + RangeLength : index;
        }

    }
}
