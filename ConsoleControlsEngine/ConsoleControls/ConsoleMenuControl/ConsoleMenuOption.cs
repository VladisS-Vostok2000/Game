using Game.ColoredCharsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleControlsEngine.Controls {
    public readonly struct ConsoleMenuOption {
        public readonly string Text;
        public readonly MulticoloredString MulticoloredText;



        public ConsoleMenuOption(MulticoloredString multicoloredText) {
            Text = multicoloredText.ToString();
            MulticoloredText = multicoloredText;
        }

    }
}
