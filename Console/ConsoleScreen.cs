using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Console;
using ExtensionMethods;

namespace Console {
    /// <summary>
    /// Инкапсулирует связь данных и консоли.
    /// </summary>
    public static class ConsoleScreen {
        public static PrivateList<ConsoleWindow> Windows { get; }



        public static void AddWindow(int x, int y, int width, int height) {
            if (!InBufferArea(new Point(x, y), new Point(x + width, y + height))) {
                throw new InvalidOperationException($"Недостаточно {nameof(BufferWidth)} или {nameof(BufferHeight)} для выполнения операции.");
            }

            Windows.Add(new ConsoleWindow(x, y, width, height));
        }



        private static bool InBufferArea(params Point[] points) {
            foreach (var point in points) {
                if (!point.X.IsInRange(0, BufferWidth)
                    || !point.Y.IsInRange(0, BufferHeight)) {
                    return false;
                }
            }
            return true;
        }

    }
}
