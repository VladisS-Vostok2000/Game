using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using static System.Console;
using static Game.ConsoleControlsEngine.ConsoleDrawing;

namespace Game.ConsoleControlsEngine {
    /// <summary>
    /// Инкапсулирует связь данных и консоли.
    /// </summary>
    public static class ConsoleScreen {
        private static readonly List<ConsoleControl> controls = new List<ConsoleControl>();
        public static IReadOnlyList<ConsoleControl> Controls { get; } = controls.AsReadOnly();
        private static readonly List<ConsoleControl> removedControls = new List<ConsoleControl>();



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

            removedControls.Add(ConsoleSingleCharPictureControl.CreateVoidConsolePicture(control.Size));
            return true;
        }

        public static void Render() {
            foreach (var rControl in removedControls) {
                rControl.VisualizeInConsole();
            }

            removedControls.Clear();

            foreach (var control in controls) {
                control.VisualizeInConsole();
            }
        }

    }
}
