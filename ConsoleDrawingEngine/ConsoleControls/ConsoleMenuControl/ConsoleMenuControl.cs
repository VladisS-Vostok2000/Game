using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.ColoredCharsEngine;
using Game.ConsoleDrawingEngine.Types;
using static Game.ColoredCharsEngine.StaticMethods.GraphicsModificate;
using Game.BasicTypesLibrary.ExtensionMethods;
using Game.ColoredCharsEngine.Types;

namespace Game.ConsoleDrawingEngine.ConsoleControls {
    public sealed class ConsoleMenuControl : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; }
        private MulticoloredString[] picture;


        private List<MulticoloredString> menuOptions;
        public IReadOnlyList<MulticoloredString> MenuOptions => menuOptions.AsReadOnly();

        public int OptionsCount => menuOptions.Count;

        private static readonly MulticoloredString uncheckedBox = new MulticoloredString("[*] ".ToColoredString());
        private static readonly MulticoloredString checkedBox = new MulticoloredString("[") + new ColoredString("*", ConsoleColor.Red) + "] ";

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



        public ConsoleMenuControl(Point location, string[] options) : this(location, options.ToMulticoloredStrings()) {

        }
        /// 
        /// <exception cref="ArgumentException"></exception>
        public ConsoleMenuControl(Point location, IList<MulticoloredString> options) : base(location) {
            if (options.Count == 0) {
                throw new ArgumentException("Меню обязано содержать пункты.");
            }

            // ISSUE: Сделать поле IList?
            menuOptions = new List<MulticoloredString>();
            menuOptions.AddRange(options);
            picture = HardRender(options);
            ConsolePicture = new ConsoleMulticoloredStringsPicture(new MulticoloredStringsPicture(picture));

            Check(selectedOptionIndex);
        }



        private static MulticoloredString[] HardRender(IList<MulticoloredString> options) {
            MulticoloredString[] picture = new MulticoloredString[options.Count];
            // REFACTORING: вынести делегат в отдельный метод?
            int maxLength = options.Max((MulticoloredString ms) => ms.Length);
            for (int i = 0; i < picture.Length; i++) {
                picture[i] = (uncheckedBox + options[i]).PadRight(maxLength);
            }
            return picture;
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



        private void Uncheck(int option) {
            picture[option] = new MulticoloredString(uncheckedBox + menuOptions[selectedOptionIndex]);
        }
        private void Check(int option) {
            picture[option] = new MulticoloredString(checkedBox + menuOptions[option]);
        }

    }
}
