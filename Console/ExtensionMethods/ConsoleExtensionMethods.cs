using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace ExtensionMethods {
    public static class ConsoleExtensionMethods {
        public static Point CursorPosition {
            get => new Point(CursorLeft, CursorTop);
            set {
                SetCursorPosition(value.X, value.Y);
            }
        }

    }
}
