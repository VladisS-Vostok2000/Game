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
        private const int maxTileMovements = 5;

        public int LengthX => LandtilesMap.GetUpperBound(0);
        public int LengthY => LandtilesMap.GetUpperBound(1);
        public int Square => LengthX * LengthY;
        public Unit[,] Units { get; }


        public Landtile[,] LandtilesMap { get; }
        public Rules Rules { get; }


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
        public bool SelectedTileContainsUnit => this[SelectedTileX, SelectedTileY].ContainsUnit;


        private int SelectedUnitX;
        private int SelectedUnitY;
        public Point SelectedUnitLocation {
            get => new Point(SelectedUnitX, SelectedUnitY);
            set {
                SelectedUnitX = value.X;
                SelectedUnitY = value.Y;
            }
        }
        public bool UnitSelected { get; private set; }
        public Point[] SelectedUnitRoadmap { get; private set; }


        public MapTileInfo this[int x, int y] => new MapTileInfo(LandtilesMap[x, y], Units[x, y]);
        public MapTileInfo this[Point point] => this[point.X, point.Y];



        public Map(Landtile[,] landtiles, Rules rules, Unit[,] units) {
            LandtilesMap = landtiles;
            Rules = rules;
            Units = units;
        }



        public ConsoleImage[,] ToConsoleImage() {
            var outArray = new ConsoleImage[LengthX, LengthY];
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    var tile = (IConsoleDrawable)Units[c, r] ?? LandtilesMap[c, r];
                    outArray[c, r] = tile.ConsoleImage; 
                }
            }
            outArray[SelectedTileX, SelectedTileY] = new ConsoleImage(SelectedTile.ToConsoleImage().Char, selectedTileColor);
            return outArray;
        }
        //public Unit GetUnit(int x, int y) => Units[x, y];
        //public Landtile Getlandtile(int x, int y) => LandtilesMap[x, y];
        //public ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandtilesMap[x, y].ConsoleImage;
        //public ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);


        public void SelectUnit() {
            if (!SelectedTileContainsUnit) {
                return;
            }

            UnitSelected = true;
            BuildSelectedUnitRoadmap();
        }
        private void BuildSelectedUnitRoadmap() {
            
        }

    }
}
