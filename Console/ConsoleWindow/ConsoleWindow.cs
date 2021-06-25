using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.ConsoleDrawing;
using static ExtensionMethods.ConsoleExtensionMethods;
using static ExtensionMethods.BasicTypesExtensionsMethods;
using ExtensionMethods;
using System.Runtime.CompilerServices;
using static System.Console;
using Core.Console;
using Core.CharWindow;
using Core.Console.ConsoleWindow;

namespace Console.ConsoleWindow {
    public sealed class ConsoleWindow : IConsoleDrawable {
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


        public List<IConsoleDrawable> Content { get; }

        public int InternalAreaWidth => WindowWidth - 2;
        public int InternalAreaHeight => WindowHeight - 2;

        public ConsolePicture ConsolePicture => throw new NotImplementedException();

        public ConsoleWindow(int windowWidth, int windowHeight) {
            WindowWidth = windowWidth >= 3? windowWidth : throw new ConsoleWindowInvalidArgumentException($"Ширина обязана быть больше 2. {nameof(windowWidth)} был {windowWidth}.", windowWidth);
            WindowHeight = windowHeight >= 3? windowHeight : throw new ConsoleWindowInvalidArgumentException($"Высота обязана быть больше 2. {nameof(windowHeight)} был {windowHeight}.", windowHeight);

            Content = new List<IConsoleDrawable>();
        }



        public ColoredString[,] RenderConsoleImage() => throw new NotImplementedException();


        /// <summary>
        /// Создаст текстовый блок, заполненный заданной строкой.
        /// Текст разрывается только по пробелу.
        /// </summary>
        public void WriteTextBox(string text, int x, int y, int width, int height) {
            if (x < 0 || x > InternalAreaWidth) {
                throw new ConsoleWindowArgumentException($"Координаты ({nameof(x)};{nameof(y)}) должны быть во внутренней области окна.", x);
            }
            if (y < 0 || y > InternalAreaHeight) {
                throw new ConsoleWindowArgumentException($"Координаты ({nameof(x)};{nameof(y)}) должны быть во внутренней области окна.", y);
            }

            try {
                TextBox textBox = new TextBox(text, width, height);
            }
            catch (TextBoxException exception) {
                throw new ConsoleWindowInvalidArgumentException($"Один из заданных параметров не позволяет создать текстовый блок: {exception.Comment}.", exception);
            }
            


        }



        private ConsoleWindowArgumentException LineWrongWidthException(string sourse) => new ConsoleWindowArgumentException($"Входящая строка должна не выходить за ширину окна.", sourse);
        private ConsoleWindowArgumentException LineRNException(string sourse) => new ConsoleWindowArgumentException($"Входящая строка не должна содержать перенос строки.", sourse);
        private ConsoleWindowException LineWrongHeightException() => new CharWindow.ConsoleWindowException($"Добавляемая строка должна быть во внутренней области окна.");


    }
}
