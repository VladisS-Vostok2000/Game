using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public static class TilesMethods {
        public static bool TilesClosely(Point tile1Coord, Point tile2Coord) =>
    (Math.Abs(tile1Coord.X - tile2Coord.X) == 1 && tile1Coord.Y == tile2Coord.Y) ||
    (Math.Abs(tile1Coord.Y - tile2Coord.Y) == 1 && tile1Coord.X == tile2Coord.X);

    }
}
