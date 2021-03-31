using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    internal static class ExistionMethods {
        internal static string ClearEmptySpaces(this string target) {
            string outString = "";
            for (int i = 0; i < target.Length; i++) {
                if (!char.IsWhiteSpace(target[i])) {
                    outString += target[i];
                }
            }
            return outString;
        }
    }
}
