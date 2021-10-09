using Game.ColoredCharsEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.BasicTypesLibrary {
    public static class CollectionsExtensions {

        #region String[]

        public static List<ColoredString> ToColoredStringsList(this string[] array, ConsoleColor color = ConsoleColor.White) {
            var outList = new List<ColoredString>();
            foreach (var str in array) {
                outList.Add(new ColoredString(str, color));
            }
            return outList;
        }
        //public static List<MulticoloredStringBuilder> ToMulticoloredStringBuildersList(this string[] array, ConsoleColor color = ConsoleColor.White) {
        //    var outList = new List<MulticoloredStringBuilder>();
        //    foreach (var str in array) {
        //        outList.Add(new MulticoloredStringBuilder(new ColoredString(str, color)));
        //    }
        //    return outList;
        //}
        
        #endregion

        #region Array
        /// <summary>
        /// Заполнит массив заданными значениями.
        /// </summary>
        /// <returns> Ссылка на текущий массив. </returns>
        public static T[] Fill<T>(this T[] array, T value) where T : struct {
            for (int i = 0; i < array.Length; i++) {
                array[i] = value;
            }
            return array;
        }
        /// <summary>
        /// Заполнит массив значением, возвращаемым заданным делегатом.
        /// </summary>
        public static T[] Fill<T>(this T[] array, Func<T> action) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = action.Invoke();
            }
            return array;
        }
        /// <summary>
        /// Создаст нередактируемое отражение <see cref="Array"/>.
        /// </summary>
        public static ReadOnlyArray<T> AsReadOnly<T>(this T[] array) => new ReadOnlyArray<T>(array);
        /// <summary>
        /// Создаст нередактируемое отражение <see cref="Array"/>.
        /// </summary>
        public static ReadOnlyDoubleDemensionArray<T> AsReadOnly<T>(this T[,] array) => new ReadOnlyDoubleDemensionArray<T>(array);
        public static bool Empty<T>(this T[] array) => array.Length == 0;

        public static bool IsEmptyOrFlat<T>(this T[,] array) {
            return array.GetUpperBound(0) == 0 || array.GetUpperBound(1) == 0;
        }
        #endregion

        #region IEnumerable<T>

        /// <summary>
        /// <see langword="true"/>, если коллекция пуста.
        /// </summary>
        public static bool Empty<T>(this IEnumerable<T> sourse) => sourse.Count() == 0;

        /// <summary>
        /// Вставляет заданный <see cref="IEnumerable{T}"/> в начало <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns> Новый объект <see cref="IEnumerable{T}"/>. </returns>
        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> enm, IEnumerable<T> insertedEnum) {
            var outEnum = new Collection<T>();
            outEnum.AddRange(insertedEnum);
            outEnum.AddRange(enm);
            return outEnum;
        }
        public static IEnumerable<T> PrependRange<T>(this IEnumerable<T> enm, IEnumerable<T> insertedEnum) {
            var outEnum = new Collection<T>();
            outEnum.AddRange(enm);
            outEnum.AddRange(insertedEnum);
            return outEnum;
        }

        #endregion

        #region ICollection, IReadOnlyCollection

        /// <summary>
        /// <see langword="true"/>, если коллекция пуста.
        /// </summary>
        public static bool Empty<T>(this IReadOnlyCollection<T> sourse) => sourse.Count == 0;

        /// <summary>
        /// <see langword="true"/>, если содержит определяемое делегатом значение.
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
        /// <see langword="true"/>, если все элементы последовательности равны между собой.
        /// </summary>
        public static bool IsEven<T>(this IList<T> list) where T : IEquatable<T> {
            if (list.Empty()) {
                throw new ArgumentException("Коллекция пуста.");
            }
            if (list.Count == 1) {
                return true;
            }

            for (int i = 0; i < list.Count - 1; i++) {
                if (!list[i].Equals(list[i + 1])) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Присвоит последнему аргументу листа значение.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetLast<T>(this IList<T> list, T value) {
            if (list.Empty()) {
                throw new InvalidOperationException("Лист пуст.");
            }
            list[list.Count - 1] = value;
        }

        #endregion

        #region IList

        /// <summary>
        /// Вставляет элемент в начало <see cref="IList{T}"/>.
        /// </summary>
        /// <returns> Ссылка на текущий экземпляр. </returns>
        public static IList<T> AddHead<T>(this IList<T> list, T item) {
            list.Insert(0, item);
            return list;
        }

        /// <summary>
        /// Добавит заданную <see cref="IEnumerable{T}"/> в начало списка.
        /// </summary>
        /// <returns> Текущий <see cref="IList{T}"/>. </returns>
        public static IList<T> AddHeadRange<T>(this IList<T> iList, IEnumerable<T> insertedHead) {
            IList<T> temp = new List<T>();
            temp.AddRange(insertedHead);
            temp.AddRange(iList);
            iList.Clear();
            iList.AddRange(temp);
            return iList;
        }

        #endregion

    }
}
