using Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undefinded {
    public static class ExtensionsMethods {
        /// <summary>
        /// True, если коллекция пуста.
        /// </summary>
        public static bool Empty<T>(this ICollection<T> sourse) => sourse.Count == 0;

        /// <summary>
        /// True, если коллекция пуста.
        /// </summary>
        public static bool Empty<T>(this IReadOnlyCollection<T> sourse) => sourse.Count == 0;

        /// <summary>
        /// Гарантирует нахождение числа в заданном диапазоне.
        /// </summary>
        public static int InRange(in this int target, in int rangeStart, in int rangeLength) {
            int rawIndex = target - rangeStart;
            int index = rawIndex % rangeLength;
            return index < 0 ? index + rangeLength : index;
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
            // Вау, какие яркие цвета!
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
        public static bool Remove<T1, T2, T3>(this IDictionary<T1, IDictionary<T2, T3>> dictionary, Predicate<IDictionary<T2, T3>> predicate) {
            foreach (var pairs in dictionary) {
                if (predicate(pairs.Value)) {
                    return dictionary.Remove(pairs);
                }
            }
            return false;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> input) {
            foreach (var item in input) {
                collection.Add(item);
            }
        }


        // REFACTORING: что вообще здесь забыла бизнес-логика? Это даже не extension.
        public static bool TilesClosely(Point tile1Coord, Point tile2Coord) =>
            (Math.Abs(tile1Coord.X - tile2Coord.X) == 1 && tile1Coord.Y == tile2Coord.Y) ||
            (Math.Abs(tile1Coord.Y - tile2Coord.Y) == 1 && tile1Coord.X == tile2Coord.X);

    }
}

