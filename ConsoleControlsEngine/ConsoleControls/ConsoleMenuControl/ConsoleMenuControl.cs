using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.ColoredCharsEngine;
using Game.ConsoleControlsEngine.Types;
using static Game.ColoredCharsEngine.StaticMethods.GraphicsModificate;
using Game.BasicTypesLibrary.Extensions;
using Game.ColoredCharsEngine.Types;

namespace Game.ConsoleControlsEngine.Controls {
    public sealed class ConsoleMenuControl : ConsoleControl {
        public override ConsolePicture ConsolePicture { get; }
        private MulticoloredString[] picture;


        private List<MulticoloredString> menuOptions;

        public int OptionsCount => menuOptions.Count;

        private static readonly MulticoloredString uncheckedBox = (MulticoloredString)(ColoredString)"[*] ";
        private static readonly MulticoloredString checkedBox = (new MulticoloredStringBuilder((ColoredString)"[") + new ColoredString("*", ConsoleColor.Red) + (ColoredString)"] ").ToMulticoloredString();

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



        /// <summary>
        /// Создаст экземпляр <see cref="ConsoleMenuControl"/> из непустого списка опций.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public ConsoleMenuControl(Point location, IEnumerable<MulticoloredString> options) : base(location) {
            if (options.Empty()) {
                throw new ArgumentException("Меню обязано содержать пункты.");
            }

            // ISSUE: Сделать поле IList?
            menuOptions = new List<MulticoloredString>();
            menuOptions.AddRange(options);
            picture = HardRender(menuOptions);
            ConsolePicture = new ConsoleMulticoloredStringsPicture(new MulticoloredStringsPicture(picture));

            Check(selectedOptionIndex);
        }



        private static MulticoloredString[] HardRender(IList<MulticoloredString> options) {
            MulticoloredString[] picture = new MulticoloredString[options.Count];
            int maxLength = options.FindLongerLength();
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
