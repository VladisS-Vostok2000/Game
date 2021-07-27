using Game.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.ConsoleEngine.ConsoleControls {
    public sealed class ConsoleMenu : ConsoleControl {
        private List<MulticoloredStringBuilder> menuOptions;
        public IReadOnlyList<MulticoloredStringBuilder> MenuOptions { get; }

        public int OptionsCount => menuOptions.Count;


        private MulticoloredStringBuilder[] picture;
        public override Picture ConsolePicture { get; protected set; }


        private readonly MulticoloredStringBuilder uncheckedBox = new MulticoloredStringBuilder("[*] ");
        private readonly MulticoloredStringBuilder checkedBox = "[" + new ColoredString("*", ConsoleColor.Red) + "] ";


        private int selectedOptionIndex = 0;
        public int SelectedOptionIndex {
            get => selectedOptionIndex;
            set {
                if (value >= picture.Length) { throw new ArgumentOutOfRangeException(); }

                Uncheck(selectedOptionIndex);
                Check(value);
            }
        }

        public ConsoleMenuOption SelectedOption => new ConsoleMenuOption(menuOptions[selectedOptionIndex]);



        // REFACTORING: ICollection вместо IList?
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
            ConsolePicture = new MulticoloredStringsPicture(picture);
        }
        public ConsoleMenu(int x, int y, int width, int height, IList<MulticoloredStringBuilder> options) : this(new Point(x,y), new Size(width,height), options) {

        }


        /// <summary>
        /// Смещает выделение пункта вверх, зацикленно.
        /// </summary>
        public void Up() {
            SelectedOptionIndex = (SelectedOptionIndex + 1).ToRange(0, OptionsCount - 1);
        }
        /// <summary>
        /// Смещает выделение пункта вниз, зацикленно.
        /// </summary>
        public void Down() {
            SelectedOptionIndex = (SelectedOptionIndex - 1).ToRange(0, OptionsCount - 1);
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
            picture[option] = new MulticoloredStringBuilder(checkedBox + menuOptions[selectedOptionIndex]);
        }

    }
}
