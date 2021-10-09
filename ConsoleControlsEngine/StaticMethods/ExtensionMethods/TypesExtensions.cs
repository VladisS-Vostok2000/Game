using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Game.ConsoleDrawingEngine;

namespace Game.ConsoleDrawingEngine {
    public static class TypesExtensions {
        public static ColoredString ToColoredString(this string str, ConsoleColor color = ConsoleColor.White) {
            return new ColoredString(str, color);
        }
        public static IEnumerable<MulticoloredString> ToMulticoloredStringsEnum(this IEnumerable<string> enm) {
            return enm.Select((string str) => (ColoredString)str).Select((ColoredString cs) => (MulticoloredString)cs);
        }
        public static int FindLongerLength(this IEnumerable<MulticoloredString> multicoloredStrings) {
            return multicoloredStrings.Max((MulticoloredString ms) => ms.Length);
        }

    }
}
