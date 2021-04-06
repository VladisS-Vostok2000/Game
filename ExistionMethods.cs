using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undefinded {
    public static class ExistionMethods {
        /// <summary>
        /// Отчистит строку от символов, относящихся к категории пробелов.
        /// </summary>
        public static string ClearEmptySpaces(this string target) {
            string outString = "";
            for (int i = 0; i < target.Length; i++) {
                if (!char.IsWhiteSpace(target[i])) {
                    outString += target[i];
                }
            }
            return outString;
        }

        /// <summary>
        /// Извлекает подстроку заданной длинны с заданной позиции.
        /// </summary>
        public static string Extract(this string target, int firstIndex, int Length) {
            string outString = "";
            for (int i = firstIndex; i < target.Length && Length-- > 0; i++) {
                outString += target[i];
            }
            return outString;
        }

        /// <summary>
        /// Гарантирует нахождение числа в заданном диапазоне.
        /// </summary>
        public static int InRange(in this int target, in int rangeStart, in int RangeLength) {
            int rawIndex = target - rangeStart;
            int index = rawIndex % RangeLength;
            return index < 0 ? index + RangeLength : index;
        }

    }
}
