using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleDrawingEngine {
    [Obsolete]
    public static class Geometry {
        public static bool PointInsideRectangle(Point point, Rectangle rectangle) {
            return point.X >= rectangle.X && point.Y < rectangle.Width &&
                point.Y >= rectangle.Y && point.Y < rectangle.Height;
        }

    }
}
