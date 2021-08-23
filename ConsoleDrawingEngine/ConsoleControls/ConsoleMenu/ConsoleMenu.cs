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

namespace Game.ConsoleDrawingEngine.Controls {
    // ISSUE: убрать "console" из имён контролов?
    public sealed class ConsoleMenu : ConsoleControl {
        private List<MulticoloredStringBuilder> menuOptions;
        public IReadOnlyList<MulticoloredStringBuilder> MenuOptions { get; }

        public int OptionsCount => menuOptions.Count;


        private MulticoloredStringBuilder[] picture;
        public override ConsolePicture ConsolePicture { get; protected set; }


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



        public ConsoleMenu(Point location, Size size, IList<MulticoloredStringBuilder> options) : base(location, size) {
            if (options.Count == 0) {
                throw new ArgumentException("Меню обязано содержать пункты.");
            }

            var menuOptions = new List<MulticoloredStringBuilder>();
            foreach (var option in options) {
                menuOptions.Add(option);
            }

            this.menuOptions = menuOptions;
            MenuOptions = this.menuOptions.AsReadOnly();
            picture = Render(menuOptions);
            ConsolePicture = new ConsoleMulticoloredStringsPicture(new MulticoloredStringsPicture(picture));
            Check(selectedOptionIndex);
        }
        public ConsoleMenu(Point location, Size size, string[] options) : this(location, size, options.ToMulticoloredStringBuilder()) {

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



        private MulticoloredStringBuilder[] Render(IList<MulticoloredStringBuilder> options) {
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
