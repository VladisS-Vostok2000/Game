using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using IniParser;
using Undefinded;

namespace Game {
    public class Map {
        public int LengthX => LandtilesMap.GetUpperBound(0);
        public int LengthY => LandtilesMap.GetUpperBound(1);
        public int Square => LengthX * LengthY;
        public Unit[,] Units { get; }


        public Landtile[,] LandtilesMap { get; }
        public Rules Rules { get; }
        public Rules MapRules { get; }


        public Point SelectedTileLocation {
            get => new Point(SelectedTileX, SelectedTileY);
            set {
                SelectedTileX = value.X;
                SelectedTileY = value.Y;
            }
        }
        private int selectedTileX;
        private int selectedTileY;
        public int SelectedTileX {
            get => selectedTileX;
            set => selectedTileX = value.InRange(0, LengthX);
        }
        public int SelectedTileY {
            get => selectedTileY;
            set => selectedTileY = value.InRange(0, LengthY);
        }
        private static readonly ConsoleColor selectedTileColor = ConsoleColor.Yellow;
        public MapTileInfo SelectedTile => this[SelectedTileX, SelectedTileY];



        public MapTileInfo this[int x, int y] => new MapTileInfo(LandtilesMap[x, y], Units[x, y]);



        public Map(Landtile[,] landtiles, Rules rules, Unit[,] units) {
            LandtilesMap = landtiles;
            Rules = rules;
            Units = units;
        }



        public ConsoleImage[,] ToConsoleImages() {
            ConsoleImage[,] outArray = new ConsoleImage[LengthX, LengthY];
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outArray[c, r] = Units[c, r]?.ConsoleImage ?? LandtilesMap[c, r].ConsoleImage;
                }
            }
            outArray[SelectedTileLocation.X, SelectedTileLocation.Y].Color = selectedTileColor;
            return outArray;
        }
        public Unit GetUnit(int x, int y) => Units[x, y];
        public Landtile Getlandtile(int x, int y) => LandtilesMap[x, y];
        public ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandtilesMap[x, y].ConsoleImage;
        public ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);

    }
}
