using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    /// <summary>
    /// Имеющий размеры и логику прорисовки консольный объект.
    /// </summary>
    public abstract class ConsoleControl : IConsoleDrawable {
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point Location { get => new Point(X, Y); set { X = value.X; Y = value.Y; } }


        public abstract Picture ConsolePicture { get; protected set; }



        public ConsoleControl(int width, int height) {
            Width = width;
            Height = height;
        }



        /// <summary>
        /// True, если области <see cref="ConsoleControl"/> пересекается.
        /// </summary>
        public bool IntersectsWith(ConsoleControl control) {
            var control1Area = new Rectangle(X, Y, Width, Height);
            var control2Area = new Rectangle(control.X, control.Y, control.Width, control.Height);
            if (control1Area.IntersectsWith(control2Area)) { return true; }
            if (control.ContainsAreaOf(this)) return true;
            return false;
        }
        /// <summary>
        /// True, если область заданного <see cref="ConsoleControl"/> полностью помещается внутри.
        /// </summary>
        public bool ContainsAreaOf(ConsoleControl control) {
            Rectangle internalArea = new Rectangle(0, 0, Width, Height);
            Rectangle controlArea = new Rectangle(X, Y, control.Width, control.Height);
            return internalArea.ContainsRectangle(controlArea);
        }

    }
}
