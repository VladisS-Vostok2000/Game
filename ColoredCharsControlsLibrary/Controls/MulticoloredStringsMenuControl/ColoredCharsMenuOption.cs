using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.ColoredCharsEngine;

namespace Game.ColoredCharsControlsLibrary {
    public class ColoredCharsMenuOption {
        public string Text { get; }
        public MulticoloredString MulticoloredText { get; }



        public ColoredCharsMenuOption(MulticoloredString multicoloredText) {
            Text = multicoloredText.ToString();
            MulticoloredText = multicoloredText;
        }

    }
}
