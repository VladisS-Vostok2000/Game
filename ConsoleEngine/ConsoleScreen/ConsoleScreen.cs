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
        private static List<IConsoleDrawable> controls = new List<IConsoleDrawable>();
        public static IReadOnlyList<IConsoleDrawable> Controls { get; } = controls.AsReadOnly();



        public static void AddControl(IConsoleDrawable control) {
            if (control == null) throw new ArgumentNullException(nameof(control));
            controls.Add(control);
        }
        public static bool RemoveControl(IConsoleDrawable control) => controls.Remove(control);


        public static void Render() {
            foreach (var control in controls) {
                // TODO:
            }
        }


        private static void DrawColoredCharPicture(ColoredCharsPicture consolePicture) { }
        private static void DrawColoredStringPicture(MulticoloredStringsPicture coloredStringsPicture) { }

    }
}
