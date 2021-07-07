using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Console;
using ExtensionMethods;
using System.Collections.ObjectModel;

namespace ConsoleEngine {
    /// <summary>
    /// Инкапсулирует связь данных и консоли.
    /// </summary>
    public static class ConsoleScreen {
        private static Collection<IConsoleControl> windows;
        public static ReadOnlyCollection<IConsoleControl> Windows { get; } = new ReadOnlyCollection<IConsoleControl>(windows);



        public static void AddControl(IConsoleControl control) {
            windows.Add(control);
        }
        //private static bool InBufferArea(params Point[] points) {
        //    foreach (var point in points) {
        //        if (!point.X.IsInRange(0, BufferWidth)
        //            || !point.Y.IsInRange(0, BufferHeight)) {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

    }
}
