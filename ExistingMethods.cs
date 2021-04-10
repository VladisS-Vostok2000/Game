using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undefinded {
    public static class ExistingMethods {
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

        /// <summary>
        /// True, если пара из заданных ключа-значения содержится в заданном словаре.
        /// </summary>
        public static bool ContainsKeyValuePair<T1, T2>(this Dictionary<T1, T2> dic, T1 key, T2 value) where T1 : IEquatable<T1> where T2 : IEquatable<T1> =>
            dic.ContainsKey(key) && dic[key].Equals(value);


        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out int result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && int.TryParse(strResult, out result);
        }
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out long result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && long.TryParse(strResult, out result);
        }
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out char result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && char.TryParse(strResult, out result);
        }
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out string result) {
            result = default;
            return pairs.TryGetValue(key, out result);
        }
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, out int result) => result = int.Parse(pairs[key]);
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, out long result) => result = long.Parse(pairs[key]);
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, out char result) => result = char.Parse(pairs[key]);
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, out string result) => result = pairs[key];

    }
}

