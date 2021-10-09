using Game.ColoredCharsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ColoredCharsControlsLibrary {
    public interface IColoredCharsMenuControl {
        IReadOnlyList<MulticoloredString> MenuOptions { get; }
        int SelectedOptionIndex { get; set; }
        ColoredCharsMenuOption SelectedOption { get; }



        /// <summary>
        /// Смещает выделение пункта вверх, зацикленно.
        /// </summary>
        void Up();
        /// <summary>
        /// Смещает выделение пункта вниз, зацикленно.
        /// </summary>
        void Down();

    }
}
