using Game.ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleEngine.ConsoleControls {
    public sealed class ConsoleMenuOption {
        public MulticoloredStringBuilder MulticoloredStringBuilderOptionName { get; }
        public string StringOptionName { get; }



        public ConsoleMenuOption(MulticoloredStringBuilder option) {
            MulticoloredStringBuilderOptionName = option;
            StringOptionName = option.ToString();
        }

    }
}
