using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods {
    public static class BasicTypesExtensionsMethods {

        /// <summary>
        /// True, если коллекция пуста.
        /// </summary>
        public static bool Empty<T>(this ICollection<T> sourse) => sourse.Count == 0;

        /// <summary>
        /// True, если коллекция пуста.
        /// </summary>
        public static bool Empty<T>(this IReadOnlyCollection<T> sourse) => sourse.Count == 0;

        /// <summary>
        /// True, если содержит определяемое делегатом значение.
        /// </summary>
        public static bool Contains<T1>(this ICollection<T1> colletion, Predicate<T1> predicate) {
            foreach (var item in colletion) {
                if (predicate(item)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Возвращает первое с начала вхождение элемента в коллекции, соответствующего заданному предикату.
        /// </summary>
        public static bool TryGet<T1>(this ICollection<T1> collection, Predicate<T1> predicate, out T1 outItem) {
            foreach (var item in collection) {
                if (predicate(item)) {
                    outItem = item;
                    return true;
                }
            }
            outItem = default;
            return false;
        }

        /// <summary>
        /// Вставляет коллекцию элементов в конец текущей.
        /// </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> input) {
            foreach (var item in input) {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Удаляет последний элемент коллекции.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void RemoveLastItem<T>(this ICollection<T> collection) {
            if (collection.Empty()) { throw new InvalidOperationException("Коллекция пуста."); }
            T removingItem = default;
            foreach (var item in collection) {
                removingItem = item;
            }
            collection.Remove(removingItem);
        }



        /// <summary>
        /// Гарантирует нахождение числа в заданном диапазоне.
        /// </summary>
        public static int ToRange(in this int target, in int rangeStart, in int rangeLength) {
            int rawIndex = target - rangeStart;
            int index = rawIndex % rangeLength;
            return index < 0 ? index + rangeLength : index;
        }

        /// <summary>
        /// True, если число лежит в заданном диапазоне включительно.
        /// </summary>
        public static bool IsInRange(in this int target, in int lowerBound, in int upperBound) => target >= lowerBound && target <= upperBound;



        /// <summary>
        /// Возвращает 0, если число меньше нуля или само значение иначе.
        /// </summary>
        public static float NotNegative(in this float target) => target < 0 ? 0 : target;



        /// <summary>
        /// True, если пара из заданных ключа-значения содержится в заданном словаре.
        /// </summary>
        public static bool ContainsKeyValuePair<T1, T2>(this IDictionary<T1, T2> dic, T1 key, T2 value) where T1 : IEquatable<T1> where T2 : IEquatable<T1> =>
            dic.ContainsKey(key) && dic[key].Equals(value);

        /// <summary>
        /// True, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out int result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && int.TryParse(strResult, out result);
        }
        /// <summary>
        /// True, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out float result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && float.TryParse(strResult, out result);
        }
        /// <summary>
        /// True, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out long result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && long.TryParse(strResult, out result);
        }
        /// <summary>
        /// True, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out char result) {
            result = default;
            return pairs.TryGetValue(key, out string strResult) && char.TryParse(strResult, out result);
        }
        /// <summary>
        /// True, если элемент содержится в коллекции и удачно спарсен.
        /// </summary>
        public static bool TryParseValue<T>(this IDictionary<T, string> pairs, T key, out string result) {
            result = default;
            return pairs.TryGetValue(key, out result);
        }
        /// <summary>
        /// True, если элемент содержится в коллекции и удачно спарсен.
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
        /// <returns> True, если элемент был удалён. </returns>
        public static bool Remove<T1, T2, T3>(this IDictionary<T1, IDictionary<T2, T3>> dictionary, Predicate<IDictionary<T2, T3>> predicate) {
            foreach (var pairs in dictionary) {
                if (predicate(pairs.Value)) {
                    return dictionary.Remove(pairs);
                }
            }
            return false;
        }



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
            if (input == null) { throw new ArgumentNullException($"{nameof(input)} был null."); }


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
        /// Заполнит массив заданными значениями.
        /// </summary>
        /// <returns> Ссылка на текущий массив. </returns>
        public static T[] Fill<T>(this T[] array, T value) where T: struct {
            for (int i = 0; i < array.Length; i++) {
                array[i] = value;
            }
            return array;
        }
        /// <summary>
        /// Заполнит массив значением, возвращаемым заданным делегатом.
        /// </summary>
        public static T[] Fill<T> (this T[] array, Func<T> action) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = action.Invoke();
            }
            return array;
        }
        /// <summary>
        /// Создаст нередактируемое отражение <see cref="Array"/>.
        /// </summary>
        public static ReadOnlyArray<T> AsReadOnly<T>(this T[] array) => new ReadOnlyArray<T>(array);


        ///// <summary>
        ///// Возвращает индекс "\r\n" c заданной позиции. -1, если не найден.
        ///// </summary>
        //public static int InfexOfRN(this string target, int startPosition = 0) {
        //    for (int i = startPosition; i < target.Length - 1; i++) {
        //        var substring = target[i].ToString() + target[i + 1].ToString();
        //        if (substring == Environment.NewLine) {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}
        ///// <summary>
        ///// Возвращает заданную строку без "/r/n".
        ///// </summary>
        //public static string RemoveRN(this string target) {
        //    var sb = new StringBuilder();

        //    for (int i = 0; i < target.Length - 1; i++) {
        //        if (target[i] == '\r' && target[i + 1] == '\n') {
        //            continue;
        //        }

        //        sb.Append(target[i]);
        //    }

        //    return sb.ToString();
        //}

    }
}

