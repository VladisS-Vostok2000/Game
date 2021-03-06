using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// REFACTORING: необходимо расчленить этот класс на несколько, потому что
// в нём собраны методы расширения со всех библиотек.
// Ожидается: ConsoleDrawingEngine.
namespace Game.BasicTypesLibrary {
    public static class BasicTypesExtensions {

        #region Int32
        /// <summary>
        /// Гарантирует нахождение числа в заданном диапазоне.
        /// </summary>
        public static int ToRange(in this int target, in int rangeStart, in int rangeLength) {
            int rawIndex = target - rangeStart;
            int index = rawIndex % rangeLength;
            return index < 0 ? index + rangeLength : index;
        }

        /// <summary>
        /// <see langword="true"/>, если число лежит в заданном диапазоне включительно.
        /// </summary>
        public static bool IsInRange(in this int target, in int lowerBound, in int upperBound) => target >= lowerBound && target <= upperBound;
        #endregion

        #region Float
        /// <summary>
        /// Возвращает 0, если число меньше нуля или само значение иначе.
        /// </summary>
        public static float NotNegative(in this float target) => target < 0 ? 0 : target;
        #endregion

        #region Point
        /// <summary>
        /// <see langword="true"/>, если заданный <see cref="Point"/> с четырёх сторон к объекту вплотную.
        /// </summary>
        public static bool CloseTo(this Point value, Point target) => !(Math.Abs(value.Y - target.Y) > 1 || Math.Abs(value.X - target.X) > 1);
        #endregion

        #region Size
        public static bool IsEmptyOrFlat(this Size size) {
            return size.Width == 0 || size.Height == 0;
        }
        #endregion

        #region Dictionary
        /// <summary>
        /// <see langword="true"/>, если пара из заданных ключа-значения содержится в заданном словаре.
        /// </summary>
        public static bool ContainsKeyValuePair<T1, T2>(this IDictionary<T1, T2> dic, T1 key, T2 value) where T1 : IEquatable<T1> where T2 : IEquatable<T1> =>
            dic.ContainsKey(key) && dic[key].Equals(value);

        /// <summary>
        /// <see langword="true"/>, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out int result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && int.TryParse(strResult, out result);
        }
        /// <summary>
        /// <see langword="true"/>, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out float result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && float.TryParse(strResult, out result);
        }
        /// <summary>
        /// <see langword="true"/>, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out long result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && long.TryParse(strResult, out result);
        }
        /// <summary>
        /// <see langword="true"/>, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out char result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && char.TryParse(strResult, out result);
        }
        /// <summary>
        /// <see langword="true"/>, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out string result) {
            result = default;
            return pairs.TryGetValue(key, out result);
        }
        /// <summary>
        /// <see langword="true"/>, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out ConsoleColor result) {
            result = default;
            return pairs.TryGetValue(key, out string value) && Enum.TryParse(value, out result);
        }

        /// <summary>
        /// Производит модификацию секций, атрибутов и значений в соответствии с заданным словарём. 
        /// </summary>
        public static void Merge<T1, T2, T3>(this IDictionary<T1, IDictionary<T2, T3>> ini1, IDictionary<T1, IDictionary<T2, T3>> ini2) {
            foreach (var section2 in ini2) {
                T1 section2Name = section2.Key;
                // Если секция уже существует.
                if (ini1.TryGetValue(section2Name, out IDictionary<T2, T3> pairs1)) {
                    T1 sectionName = section2Name;
                    IDictionary<T2, T3> section2Pairs = section2.Value;
                    foreach (var section2Pair in section2Pairs) {
                        // Если ключ существует.
                        // REFACTORING: рассмотреть вариант без exception.
                        try {
                            pairs1[section2Pair.Key] = section2Pair.Value;
                        }
                        catch (KeyNotFoundException) {
                            pairs1.Add(section2Pair.Key, section2Pair.Value);
                        }
                    }

                }
                else {
                    ini1.Add(section2Name, section2.Value);
                }
            }
        }

        /// <summary>
        /// Удаляет определяемый делегатом элемент.
        /// </summary>
        /// <returns> <see langword="true"/>, если элемент был удалён. </returns>
        public static bool Remove<T1, T2, T3>(this IDictionary<T1, IDictionary<T2, T3>> dictionary, Predicate<IDictionary<T2, T3>> predicate) {
            foreach (var pairs in dictionary) {
                if (predicate(pairs.Value)) {
                    return dictionary.Remove(pairs);
                }
            }
            return false;
        }
        #endregion

        #region String
        /// <summary>
        /// Отчистит строку от символов, относящихся к категории пробелов.
        /// </summary>
        public static string ClearEmptySpaces(this string target) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < target.Length; i++) {
                if (!char.IsWhiteSpace(target[i])) {
                    sb.Append(target[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Извлекает подстроку с заданной позиции, пока не будет встречен заданный символ.
        /// Возвращается строка содержащая символ со стартовой индексацией, но без заданного.
        /// </summary>
        public static string Substring(this string target, int startIndex, char chr) {
            int lastIndex = target.IndexOf(chr, startIndex);
            int length = lastIndex - startIndex;
            return target.Substring(startIndex, length);
        }

        /// <summary>
        /// Извлекает подстроку с заданной позиции, пока не будет встречен символ, относящийся к категории пробелов.
        /// </summary>
        public static string ExtractWord(this string target, int startIndex = 0) {
            int whiteSpaceIndex = -1;
            for (int i = startIndex; i < target.Length; i++) {
                char letter = target[i];
                if (char.IsWhiteSpace(letter)) {
                    whiteSpaceIndex = i;
                    break;
                }
            }

            if (whiteSpaceIndex == -1) {
                return target.Substring(startIndex);
            }

            int length = whiteSpaceIndex - startIndex;
            return target.Substring(startIndex, length);
        }

        /// <summary>
        /// Возвращает строку, в которой все заданные символы отсутствуют.
        /// </summary>
        public static string Remove(this string target, char aim) {
            var sb = new StringBuilder();
            foreach (var letter in target) {
                if (letter == aim) { continue; }

                sb.Append(letter);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Возвращает проходящий по строкам перечислитель заданного текста.
        /// </summary>
        public static IEnumerable<string> SplitToLines(this string input, StringSplitOptions stringSplitOptions = StringSplitOptions.None) {
            if (input is null) { throw new ArgumentNullException($"{nameof(input)} был null."); }


            using (var reader = new StringReader(input)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (stringSplitOptions == StringSplitOptions.RemoveEmptyEntries && line == "") {
                        continue;
                    }

                    yield return line;
                }
            }
        }

        /// <summary>
        /// Возвращает индекс символа(-ов), относящихся к переносу строки.
        /// -1, если не найден.
        /// </summary>
        public static int IndexOfNewLine(this string value) => value.IndexOf(Environment.NewLine);

        /// <summary>
        /// Возвращает индекс символа(-ов), относящихся к переносу строки.
        /// -1, если не найден.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int IndexOfNewLine(this string value, int startIndex) => value.IndexOf(Environment.NewLine, startIndex);

        /// <summary>
        /// <see langword="true"/>, если содержит символ переноса строки.
        /// </summary>
        public static bool ContainsNewLine(this string value) => value.IndexOfNewLine() != -1;

        /// <summary>
        /// Возвращает подстроку заданной длинны. Вернётся полная строка при избыточной длинне.
        /// Вернётся пустая строка при длинне 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string StringPart(this string value, int length) {
            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length), "Длинна не может быть отрицательной.");
            }

            if (length <= value.Length) {
                return value.Substring(0, length);
            }

            return value.Substring(0, value.Length);
        }

        #endregion

    }
}

