using Game.BasicTypesLibrary.ExtensionMethods;
using Game.ColoredCharsEngine.Types;
using System;
using System.Collections.Generic;
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
            int maxWidth = 0;
            foreach (var str in strings) {
                maxWidth = Math.Max(maxWidth, str.Length);
            }
            string[] outArray = new string[strings.Length];
            for (int i = 0; i < strings.Length; i++) {
                outArray[i] = strings[i].PadRight(maxWidth);
            }

            return outArray;
        }
        // TASK: объеденить это под enumerable.
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(MulticoloredStringBuilder[] mSBs) {
            bool single = CheckNullOrEmptyOrSingle(mSBs);
            if (single) {
                return true;
            }

            return IsRectangular((MulticoloredStringBuilder mSB) => mSB.Length, mSBs);
        }
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(MulticoloredString[] mSs) {
            bool single = CheckNullOrEmptyOrSingle(mSs);
            if (single) {
                return true;
            }

            return IsRectangular((MulticoloredString mS) => mS.Length, mSs);
        }
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(IList<MulticoloredStringBuilder> mSBs) {
            bool single = CheckNullOrEmptyOrSingle(mSBs);
            if (single) {
                return true;
            }

            return IsRectangular((MulticoloredStringBuilder mSB) => mSB.Length, mSBs);
        }
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(IList<MulticoloredString> ms) {
            bool single = CheckNullOrEmptyOrSingle(ms);
            if (single) {
                return true;
            }

            return IsRectangular((MulticoloredString mss) => mss.Length, ms);
        }
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(string[] strings) {
            return IsRectangular(new List<string>(strings));
        }
        /// <summary>
        /// <see langword="true"/>, если заданный массив на печати прямоуголен или пуст.
        /// </summary>
        public static bool IsRectangular(IList<string> strings) {
            bool single = CheckNullOrEmptyOrSingle(strings);
            if (single) {
                return true;
            }

            return IsRectangular((string str) => str.Length, strings);
        }
        private static bool CheckNullOrEmptyOrSingle<T>(ICollection<T> collection) {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }
            if (collection.Count() == 0) {
                throw new ArgumentException("Массив пуст.");
            }
            if (collection.Count() == 1) {
                return true;
            }
            return false;
        }
        private static bool IsRectangular<T>(Func<T, int> func, IList<T> list) {
            int width = func(list[0]);
            for (int i = 1; i < list.Count; i++) {
                if (func(list[i]) != width) {
                    return false;
                }
            }
            return true;
        }

    }
}
