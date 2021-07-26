using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Console;
using ExtensionMethods;
using static ExtensionMethods.ConsoleExtensionMethods;
using System.Collections.ObjectModel;

namespace ConsoleEngine {
    /// <summary>
    /// Инкапсулирует связь данных и консоли.
    /// </summary>
    public static class ConsoleScreen {
        private static List<IConsoleControl> controls = new List<IConsoleControl>();
        public static IReadOnlyList<IConsoleControl> Controls { get; } = controls.AsReadOnly();



        public static void AddControl(IConsoleControl control) {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            controls.Add(control);
        }
        public static bool RemoveControl(IConsoleControl control) => controls.Remove(control);


        public static void Render() {
            foreach (var control in controls) {
                Draw(control);
            }
        }

    }
}
