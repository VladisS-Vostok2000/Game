using Game.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
