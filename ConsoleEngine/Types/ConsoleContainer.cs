﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public abstract class ConsoleContainer : ConsoleControl {
        public abstract IReadOnlyList<ConsoleControl> Controls { get; }



        public ConsoleContainer(int x, int y, int width, int height) : base(x, y, width, height) { }



        public abstract void AddControl(ConsoleControl control);
        public abstract bool RemoveControl(ConsoleControl control);


        /// <summary>
        /// False, если заданный <see cref="ConsoleControl"/> не имеет пересечения с внутренними <see cref="ConsoleControl"/>.
        /// </summary>
        public bool IntersectsWintControls(ConsoleControl control) {
            foreach (var internalControl in Controls) {
                if (control.IntersectsWith(internalControl)) {
                    return true;
                }
            }
            return false;
        }

    }
}
