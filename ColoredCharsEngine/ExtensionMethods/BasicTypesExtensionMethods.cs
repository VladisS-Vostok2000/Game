using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine;

namespace Game.ColoredCharsEngine {
    public static class BasicTypesExtensionMethods {
        public static ColoredString ToColoredString(this string str, ConsoleColor color = ConsoleColor.White) {
            return new ColoredString(str, color);
        }

    }
}
