using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Console;
using Game.ExtensionMethods;
using static Game.ExtensionMethods.ConsoleExtensionMethods;
using System.Collections.ObjectModel;
using static Game.ConsoleDrawingEngine.ConsoleDrawing;
using Game.ConsoleDrawingEngine.Types;
using Game.ConsoleDrawingEngine.ConsoleControls;

namespace Game.ConsoleDrawingEngine {
    /// <summary>
    /// Инкапсулирует связь данных и консоли.
    /// </summary>
    public static class ConsoleScreen {
        private static readonly List<IConsoleDrawable> controls = new List<IConsoleDrawable>();
        public static IReadOnlyList<IConsoleDrawable> Controls { get; } = controls.AsReadOnly();
        private static readonly List<ConsoleVoidControl> removedControls = new List<ConsoleVoidControl>();



        public static void AddControl(ConsoleControl control) {
            // TASK: проверка на пересечение контролов на экране.
            if (control is null) {
                throw new ArgumentNullException(nameof(control));
            }

            controls.Add(control);
        }
        public static bool RemoveControl(ConsoleControl control) {
            bool removed = controls.Remove(control);
            if (!removed) {
                return false;
            }

            removedControls.Add(new ConsoleVoidControl(control.Location, control.Size));
            return true;
        }

        public static void Render() {
            foreach (var rControl in removedControls) {
                rControl.VisualizeInConsole();
            }

            foreach (var control in controls) {
                control.VisualizeInConsole();
            }
        }

    }
}
