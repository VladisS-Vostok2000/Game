﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExtensionMethods.ConsoleExtensionMethods;
using static ExtensionMethods.BasicTypesExtensionsMethods;
using static ConsoleEngine.Geometry;
using static ConsoleEngine.SystemDrawingExtensionMethods;
using System.Runtime.CompilerServices;

namespace ConsoleEngine {
    public sealed class ConsolePanel : ConsoleContainer {
        private List<ConsoleControl> controls;
        public override IReadOnlyList<ConsoleControl> Controls => controls.AsReadOnly();


        private ColoredChar[,] coloredChars;
        public override Picture ConsolePicture { protected set; get; }



        public ConsolePanel(int x, int y, int width, int height) : base(x, y, width, height) {
            // TODO: вынести в ColoredCharWindow
            //if (windowWidth < 3) { throw new ColoredCharPanelInvalidArgumentException($"Ширина обязана быть больше 2. {nameof(windowWidth)} был {windowWidth}.", windowWidth); }
            //if (windowHeight < 3) { throw new ColoredCharPanelInvalidArgumentException($"Высота обязана быть больше 2. {nameof(windowHeight)} был {windowHeight}.", windowHeight); }
            Width = width;
            Height = height;
            controls = new List<ConsoleControl>();
            coloredChars = new ColoredChar[Width, Height];
            ConsolePicture = new ColoredCharsPicture(coloredChars);
        }



        public override void AddControl(ConsoleControl control) {
            bool valid = ContainsAreaOf(control) && !IntersectsWintControls(control);
            if (!valid) {
                throw new ColoredCharPanelInvalidArgumentException("Заданный контрол находится за пределами дочернего или пересекает его границу.", control);
            }

            controls.Add(control);
        }
        public override bool RemoveControl(ConsoleControl control) => controls.Remove(control);

    }
}