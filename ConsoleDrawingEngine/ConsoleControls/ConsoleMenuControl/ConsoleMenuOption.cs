using Game.ConsoleDrawingEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine;

namespace Game.ConsoleDrawingEngine.Controls {
    // TASK: удалить ненужный класс.
    public sealed class ConsoleMenuOption {
        public MulticoloredString MulticoloredStringOptionName { get; }
        public string StringOptionName { get; }



        public ConsoleMenuOption(MulticoloredString option) {
            MulticoloredStringOptionName = option;
            StringOptionName = option.ToString();
        }

    }
}
