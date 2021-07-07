using ConsoleEngine;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.Core.ConsoleEngineControls {
    public sealed class ConsoleMenu : IConsoleControl {
        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// Ширина, соответсвующая 1 символу строки меню.
        /// </summary>
        public const int MinWidth = 5;
        public int MenuStringLength => Width - MinWidth + 1;
        private string[] MenuOptions { get; }


        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ColoredChar[,] coloredChars;
        public ConsolePicture ColoredCharPicture { get; }



        public ConsoleMenu(int width, string[] menuStrings) {
            // TODO: добавить исключения.
            //if (width < MinWidth) {
            //    throw new ConsoleMenuInvalidArgumentException("")
            //}
            Width = width;
            Height = menuStrings.Length;
            MenuOptions = new string[Height];
            for (int i = 0; i < Height; i++) {
                // TODO: добавить исключение.
                if (menuStrings[i] == null) throw new Exception();

                MenuOptions[i] = menuStrings[i].StringPart(MenuStringLength).PadRight(MenuStringLength);
            }
            coloredChars = new ColoredChar[Width, Height];
            ColoredCharPicture = new ConsolePicture(coloredChars);
            Render();
        }



        public void Render() {
            var temp = "[*] " + MenuOptions[0];

        }

    }
}
