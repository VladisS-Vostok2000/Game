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
    public sealed class Map {
        private const int speedPerTile = 20;
        private const int timePerTurn = 5;


        private const ConsoleColor unitRoadmapColor = ConsoleColor.Cyan;


        public int LengthX => LandtilesMap.GetUpperBound(0) + 1;
        public int LengthY => LandtilesMap.GetUpperBound(1) + 1;
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


        public Unit SelectedUnit { get; private set; }
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
        public bool[,] SelectedUnitRoadmap { get; private set; }


        public MapTileInfo this[int x, int y] => new MapTileInfo(LandtilesMap[x, y], Units[x, y]);
        public MapTileInfo this[Point point] => this[point.X, point.Y];



        public Map(Landtile[,] landtiles, Rules rules, Unit[,] units) {
            LandtilesMap = landtiles;
            Rules = rules;
            Units = units;
        }



        public ConsoleImage[,] ToConsoleImages() {
            if (UnitSelected) {
                BuildSelectedUnitRoadmap();
            }

            var outArray = new ConsoleImage[LengthX, LengthY];
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    IConsoleDrawable tile = (IConsoleDrawable)Units[c, r] ?? LandtilesMap[c, r];
                    char tileChar = tile.ConsoleImage.Char;
                    ConsoleImage outTile = UnitSelected && SelectedUnitRoadmap[c, r] ?
                        new ConsoleImage(tileChar, unitRoadmapColor) : tile.ConsoleImage;
                    outArray[c, r] = outTile;
                }
            }

            // Подсвеченный тайл.
            outArray[SelectedTileX, SelectedTileY] = new ConsoleImage(SelectedTile.ToConsoleImage().Char, selectedTileColor);

            return outArray;
        }
        //public Unit GetUnit(int x, int y) => Units[x, y];
        //public Landtile Getlandtile(int x, int y) => LandtilesMap[x, y];
        //public ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandtilesMap[x, y].ConsoleImage;
        //public ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);


        public void SelectUnit(Point location) {
            var selectedTile = this[location];
            if (!selectedTile.ContainsUnit) {
                return;
            }

            SelectedUnit = selectedTile.Unit;
            SelectedUnitLocation = location;
            UnitSelected = true;
            BuildSelectedUnitRoadmap();
        }
        private void BuildSelectedUnitRoadmap() {
            SelectedUnitRoadmap = new bool[LengthX, LengthY];
            Unit unit = SelectedUnit;
            // TODO: подключить многопоточность?
            SelectRoadmapTiles(SelectedUnitX + 1, SelectedUnitY, timePerTurn);
            SelectRoadmapTiles(SelectedUnitX - 1, SelectedUnitY, timePerTurn);
            SelectRoadmapTiles(SelectedUnitX, SelectedUnitY + 1, timePerTurn);
            SelectRoadmapTiles(SelectedUnitX, SelectedUnitY - 1, timePerTurn);
        }
        private void SelectRoadmapTiles(int x, int y, double timeReserve) {
            bool tileExists = TryGetTile(x, y, out MapTileInfo landtileInfo);
            if (!tileExists) {
                return;
            }

            Landtile landtile = landtileInfo.Land;
            string landtileName = landtile.Name;
            float unitSpeed = SelectedUnit.CalculateSpeed(landtileName);
            float timeSpent = 1 / (unitSpeed / speedPerTile);
            if (timeSpent > timeReserve) {
                return;
            }

            SelectedUnitRoadmap[x, y] = true;
            timeReserve -= timeSpent;
            // TODO: подключить многопоточность?
            SelectRoadmapTiles(x + 1, y, timeReserve);
            SelectRoadmapTiles(x - 1, y, timeReserve);
            SelectRoadmapTiles(x, y + 1, timeReserve);
            SelectRoadmapTiles(x, y - 1, timeReserve);
        }
        private bool TryGetTile(int landtileX, int landtileY, out MapTileInfo landtile) {
            landtile = landtileX.IsInRange(0, LengthX - 1) && landtileY.IsInRange(0, LengthY - 1) ? this[landtileX, landtileY] : default;
            return landtile != null;
        }
        public void UnselectUnit() => UnitSelected = false;

    }
}
