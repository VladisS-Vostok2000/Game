using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExtensionMethods.ConsoleExtensionMethods;
using static ExtensionMethods.BasicTypesExtensionsMethods;
using System.Runtime.CompilerServices;

namespace ConsoleEngine {
    public sealed class ConsoleWindow : IConsoleControl {
        public Point Location {
            get => new Point(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }
        public int X { get; set; }
        public int Y { get; set; }


        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }


        public List<IConsoleControl> InternalArea { get; }

        public int InternalAreaWidth => WindowWidth - 2;
        public int InternalAreaHeight => WindowHeight - 2;


        ColoredChar[,] coloredChars;
        public ConsolePicture ColoredCharPicture { get; }


        
        public ConsoleWindow(int windowWidth, int windowHeight) {
            WindowWidth = windowWidth >= 3? windowWidth : throw new ConsoleWindowInvalidArgumentException($"Ширина обязана быть больше 2. {nameof(windowWidth)} был {windowWidth}.", windowWidth);
            WindowHeight = windowHeight >= 3? windowHeight : throw new ConsoleWindowInvalidArgumentException($"Высота обязана быть больше 2. {nameof(windowHeight)} был {windowHeight}.", windowHeight);
            InternalArea = new List<IConsoleControl>();
            coloredChars = new ColoredChar[WindowWidth, WindowHeight];
            ColoredCharPicture = new ConsolePicture(coloredChars);
        }



        private ConsoleWindowInvalidArgumentException LineWrongWidthException(string sourse) => new ConsoleWindowInvalidArgumentException($"Входящая строка должна не выходить за ширину окна.", sourse);
        private ConsoleWindowInvalidArgumentException LineRNException(string sourse) => new ConsoleWindowInvalidArgumentException($"Входящая строка не должна содержать перенос строки.", sourse);
        private ConsoleWindowException LineWrongHeightException() => new ConsoleWindowException($"Добавляемая строка должна быть во внутренней области окна.");

    }
}
