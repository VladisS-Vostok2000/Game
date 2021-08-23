using Game.ConsoleDrawingEngine.Types;
using Game.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ConsoleDrawingEngine.ConsoleDrawing;
using static Game.ExtensionMethods.BasicTypesExtensionsMethods;
using static Game.ExtensionMethods.ConsoleExtensionMethods;
using static System.Console;
namespace Game.ConsoleDrawingEngine.ConsoleControls {
    public sealed class VoidPicture : IConsoleDrawable {
        public Point Location { get; }
        public Size Size { get; }
        
        
        
        public VoidPicture(Point location, Size size) {
            Location = location;
            Size = size.IsEmptyOrFlat() ? throw new ArgumentException("Неверный размер.") : size;
        }



        public void VisualizeInConsole() {
            CursorTop = Location.Y;
            string emptyString = new string(' ', Size.Width);
            for (int i = 0; i < Size.Height; i++) {
                CursorLeft = Location.X;
                CursorTop += i;
                ConsoleDrawing.Write(emptyString);
            }

        }

    }
}
