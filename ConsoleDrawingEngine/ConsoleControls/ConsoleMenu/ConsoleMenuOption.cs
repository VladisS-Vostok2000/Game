using Game.ConsoleDrawingEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine;

namespace Game.ConsoleDrawingEngine.Controls {
    public sealed class ConsoleMenuOption {
        // TASK: MSB заменить на MS, CMO сделать структурой.
        public MulticoloredStringBuilder MulticoloredStringBuilderOptionName { get; }
        public string StringOptionName { get; }



        public ConsoleMenuOption(MulticoloredStringBuilder option) {
            MulticoloredStringBuilderOptionName = option;
            StringOptionName = option.ToString();
        }

    }
}
