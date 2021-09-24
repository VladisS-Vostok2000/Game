using Game.BasicTypesLibrary.Extensions;
using Game.ColoredCharsEngine.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.ColoredCharsEngine.StaticMethods {
    public static class GraphicsModificate {
        /// <summary>
        /// Все строки становятся одинаковой длинны, равной самой большой строке.
        /// </summary>
        public static string[] PadRight(string[] strings) {
            int maxWidth = strings.Max((string str) => str.Length);
            string[] outArray = new string[strings.Length];
            for (int i = 0; i < strings.Length; i++) {
                outArray[i] = strings[i].PadRight(maxWidth);
            }

            return outArray;
        }
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(IEnumerable<MulticoloredString> ms) {
            CheckNullOrEmpty(ms);
            if (ms.Count() == 1) {
                return true;
            }

            return IsRectangular((MulticoloredString _ms) => _ms.Length, ms);
        }
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(IEnumerable<string> strings) {
            CheckNullOrEmpty(strings);
            if (strings.Count() == 1) {
                return true;
            }

            return IsRectangular((string str) => str.Length, strings);
        }



        private static bool IsRectangular<T>(Func<T, int> func, IEnumerable<T> enm) {
            if (enm.Empty()) {
                throw new InvalidEnumArgumentException("Перечисление пустое.");
            }

            var width = func(enm.First());
            foreach (var item in enm) {
                int itemWidth = func(item);
                if (itemWidth != width) {
                    return false;
                }
            }

            return true;
        }


        private static void CheckNullOrEmpty<T>(IEnumerable<T> collection) {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }
            if (collection.Empty()) {
                throw new ArgumentException("Массив пуст.");
            }
        }

    }
}
