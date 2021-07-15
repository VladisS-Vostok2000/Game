using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public static class MapMethods {
        public static bool TilesClosely(Point tiles1Coord, Point tiles2Coord) {
            return (Math.Abs(tiles1Coord.X - tiles2Coord.X) == 1 && tiles1Coord.Y == tiles2Coord.Y) ||
            (Math.Abs(tiles1Coord.Y - tiles2Coord.Y) == 1 && tiles1Coord.X == tiles2Coord.X);
        }

    }
}
