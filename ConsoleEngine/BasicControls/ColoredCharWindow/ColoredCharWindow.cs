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
    public sealed class ColoredCharWindow : IConsoleDrawable {
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


        public List<IConsoleDrawable> InternalArea { get; }

        public int InternalAreaWidth => WindowWidth - 2;
        public int InternalAreaHeight => WindowHeight - 2;


        ColoredChar[,] coloredChars;
        public Picture ConsolePicture { get; }



        public ColoredCharWindow(int windowWidth, int windowHeight) {
            WindowWidth = windowWidth >= 3 ? windowWidth : throw new ColoredCharWindowInvalidArgumentException($"Ширина обязана быть больше 2. {nameof(windowWidth)} был {windowWidth}.", windowWidth);
            WindowHeight = windowHeight >= 3 ? windowHeight : throw new ColoredCharWindowInvalidArgumentException($"Высота обязана быть больше 2. {nameof(windowHeight)} был {windowHeight}.", windowHeight);
            InternalArea = new List<IConsoleDrawable>();
            coloredChars = new ColoredChar[WindowWidth, WindowHeight];
            ConsolePicture = new ColoredCharsPicture(coloredChars);
        }



        private ColoredCharWindowInvalidArgumentException LineWrongWidthException(string sourse) => new ColoredCharWindowInvalidArgumentException($"Входящая строка должна не выходить за ширину окна.", sourse);
        private ColoredCharWindowInvalidArgumentException LineRNException(string sourse) => new ColoredCharWindowInvalidArgumentException($"Входящая строка не должна содержать перенос строки.", sourse);
        private ColoredCharWindowException LineWrongHeightException() => new ColoredCharWindowException($"Добавляемая строка должна быть во внутренней области окна.");

    }
}
