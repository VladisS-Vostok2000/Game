﻿using System.Drawing;
using Game.ConsoleDrawingEngine.Types;

namespace Game.ConsoleDrawingEngine.Types {
    /// <summary>
    /// Имеющий размеры и логику прорисовки консольный объект.
    /// </summary>
    public abstract class ConsoleControl : IConsoleDrawable {
        public int Width => Size.Width;
        public int Height => Size.Height;
        public Size Size { get; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point Location { get => new Point(X, Y); set { X = value.X; Y = value.Y; } }


        public virtual ConsolePicture ConsolePicture { get; protected set; }



        protected ConsoleControl(Point location, Size size) {
            Location = location;
            Size = size;
        }



        ///// <summary>
        ///// True, если области <see cref="ConsoleControl"/> пересекается.
        ///// </summary>
        //public bool IntersectsWith(ConsoleControl control) {
        //    var control1Area = new Rectangle(X, Y, Width, Height);
        //    var control2Area = new Rectangle(control.X, control.Y, control.Width, control.Height);
        //    if (control1Area.IntersectsWith(control2Area)) { return true; }
        //    if (control.ContainsAreaOf(this))
        //        return true;
        //    return false;
        //}
        ///// <summary>
        ///// True, если область заданного <see cref="ConsoleControl"/> полностью помещается внутри.
        ///// </summary>
        //public bool ContainsAreaOf(ConsoleControl control) {
        //    Rectangle internalArea = new Rectangle(0, 0, Width, Height);
        //    Rectangle controlArea = new Rectangle(X, Y, control.Width, control.Height);
        //    return internalArea.ContainsRectangle(controlArea);
        //}


        public void VisualizeInConsole() {
            ConsolePicture.VisualizeInConsole(Location);
        }

    }
}