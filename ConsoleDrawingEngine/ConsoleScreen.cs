﻿using System;
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

namespace Game.ConsoleDrawingEngine {
    /// <summary>
    /// Инкапсулирует связь данных и консоли.
    /// </summary>
    public static class ConsoleScreen {
        private static List<IConsoleDrawable> controls = new List<IConsoleDrawable>();
        public static IReadOnlyList<IConsoleDrawable> Controls { get; } = controls.AsReadOnly();



        public static void AddControl(ConsoleControl control) {
            if (control == null) {
                throw new ArgumentNullException(nameof(control));
            }

            controls.Add(control);
        }
        public static bool RemoveControl(ConsoleControl control) => controls.Remove(control);


        public static void Render() {
            foreach (var control in controls) {
                control.VisualizeInConsole();
            }
        }

    }
}