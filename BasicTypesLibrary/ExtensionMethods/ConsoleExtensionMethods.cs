using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Game.BasicTypesLibrary.ExtensionMethods {
    public static class ConsoleExtensionMethods {
        public static Point CursorPosition {
            get => new Point(CursorLeft, CursorTop);
            set {
                SetCursorPosition(value.X, value.Y);
            }
        }
        public static ConsoleKey ListenKey() {
            ConsoleKeyInfo input = ReadKey(true);
            return input.Key;
        }

    }
}
