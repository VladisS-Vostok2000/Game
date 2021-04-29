using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undefinded {
    public static class ExtensionsMethods {
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
        /// True, если число лежит в заданном диапазоне.
        /// </summary>
        public static bool IsInRange(in this int target, in int lowerBound, in int upperBound) => target >= lowerBound && target <= upperBound;

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
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, ref int result) => result = int.Parse(pairs[key]);
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, ref long result) => result = long.Parse(pairs[key]);
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, ref char result) => result = char.Parse(pairs[key]);
        //public static void ParseValue<T>(this Dictionary<T, string> pairs, T key, ref string result) => result = pairs[key];


        //public static bool TryFindAndRemove<T>(this ICollection<T> collection, T target) where T: IComparable<T> {
        //    foreach (var item in collection) {
        //        if (target.Equals(item)) {
        //            collection.Remove(item);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        ///// <exception cref="KeyNotFoundException"></exception>
        //public static bool FindAndRemove<T>(this ICollection<T> collection, T target) where T : IComparable<T> {
        //    foreach (var item in collection) {
        //        if (target.Equals(item)) {
        //            collection.Remove(item);
        //            return true;
        //        }
        //    }
        //    throw new KeyNotFoundException();
        //}
        
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
        public static bool Remove<T1, T2, T3>(this IDictionary<T1, IDictionary<T2, T3>> dictionary, Predicate<IDictionary<T2,T3>> predicate) {
            foreach (var pairs in dictionary) {
                if (predicate(pairs.Value)) {
                    return dictionary.Remove(pairs);
                }
            }
            return false;
        }

        public static bool Empty<T>(this ICollection<T> collection) => collection.Count == 0;
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> input) {
            foreach (var item in input) {
                collection.Add(item);
            }
        }

    }
}

