using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.ExtensionMethods;
using Game.ColoredCharsEngine;
using Game.ConsoleDrawingEngine.Types;
using static Game.ColoredCharsEngine.StaticMethods.GraphicsModificate;

namespace Game.ConsoleDrawingEngine.Controls {
    // TASK: переименовать контролы в Console + name + Control.
    public sealed class ConsoleMenu : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; }
        private MulticoloredStringBuilder[] picture;


        private List<MulticoloredStringBuilder> menuOptions;
        public IReadOnlyList<MulticoloredStringBuilder> MenuOptions => menuOptions.AsReadOnly();

        public int OptionsCount => menuOptions.Count;

        private static readonly MulticoloredStringBuilder uncheckedBox = new MulticoloredStringBuilder("[*] ");
        private static readonly MulticoloredStringBuilder checkedBox = new MulticoloredStringBuilder("[") + new ColoredString("*", ConsoleColor.Red) + "] ";

        private int selectedOptionIndex = 0;
        public int SelectedOptionIndex {
            get => selectedOptionIndex;
            set {
                if (value >= OptionsCount) { throw new ArgumentOutOfRangeException(); }

                Uncheck(selectedOptionIndex);
                selectedOptionIndex = value;
                Check(selectedOptionIndex);
            }
        }

        public ConsoleMenuOption SelectedOption => new ConsoleMenuOption(menuOptions[selectedOptionIndex]);



        public ConsoleMenu(Point location, IList<MulticoloredStringBuilder> options) : base(location) {
            picture = Render(options);
            ConsolePicture = new ConsoleMulticoloredStringsPicture(new MulticoloredStringsPicture(picture));

            var menuOptions = new List<MulticoloredStringBuilder>();
            foreach (var option in options) {
                menuOptions.Add(option);
            }
            this.menuOptions = menuOptions;

            Check(selectedOptionIndex);
        }
        public ConsoleMenu(Point location, string[] options) : this(location, options.ToMulticoloredStringBuilder()) {

        }



        /// <summary>
        /// Смещает выделение пункта вверх, зацикленно.
        /// </summary>
        public void Up() {
            SelectedOptionIndex = (SelectedOptionIndex + 1).ToRange(0, OptionsCount);
        }
        /// <summary>
        /// Смещает выделение пункта вниз, зацикленно.
        /// </summary>
        public void Down() {
            SelectedOptionIndex = (SelectedOptionIndex - 1).ToRange(0, OptionsCount);
        }



        private static MulticoloredStringBuilder[] Render(IList<MulticoloredStringBuilder> options) {
            if (options.Count == 0) {
                throw new ArgumentException("Меню обязано содержать пункты.");
            }

            // TASK: вынести повторяющиеся исключения в throwHelper.
            if (!IsRectangular(options)) {
                throw new ArgumentException("Строки должны прорисовываться как прямоугольник.");
            }


            var picture = new MulticoloredStringBuilder[options.Count];
            picture[0] = checkedBox + options[0];
            for (int i = 1; i < picture.Length; i++) {
                picture[i] = uncheckedBox + options[i];
            }


            return picture;
        }


        private void Uncheck(int option) {
            picture[option] = new MulticoloredStringBuilder(uncheckedBox + menuOptions[selectedOptionIndex]);
        }
        private void Check(int option) {
            picture[option] = new MulticoloredStringBuilder(checkedBox + menuOptions[option]);
        }

    }
}
