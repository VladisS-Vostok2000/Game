using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using IniParcer;
using Undefinded;

namespace Game {
    public class Map {
        public int LengthX { get; }
        public int LengthY { get; }
        public int Length => LengthX * LengthY;

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

        public LandTile[,] LandTiles { get; }
        public Unit[,] Units { get; }

        private const string iniSectionMap = "Map";
        private const string iniKeyMap = "Map";
        private const string iniKeyMapLengthX = "LengthX";
        private const string iniKeyMapLengthY = "LengthY";
        private const string iniKeyUnitName = "Name";
        private const string iniKeyUnitX = "X";
        private const string iniKeyUnitY = "Y";
        private const string iniKeyUnitImageChar = "CharImage";



        public Map(string mapPath) {
            using (var streamReader = new StreamReader(mapPath)) {
                Dictionary<string, Dictionary<string, string>> ini = Parcer.Parse(mapPath);
                LengthX = int.Parse(ini[iniSectionMap][iniKeyMapLengthX]);
                LengthY = int.Parse(ini[iniSectionMap][iniKeyMapLengthY]);

                LandTiles = ParceLandTiles(ini[iniSectionMap][iniKeyMap]);
                Units = ParceUnits(ini);
            }
        }



        public MapTileInfo this[int x, int y] => new MapTileInfo(LandTiles[x, y], Units[x, y]);



        private LandTile[,] ParceLandTiles(string chars) {
            var outTiles = new LandTile[LengthX, LengthY];
            int i = 0;
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    LandTile.LandTileTypes landTileType = LandTile.CharToLandTileType(chars[i++]);
                    outTiles[c, r] = landTileType != LandTile.LandTileTypes.None? new LandTile(landTileType) : throw new Exception();
                }
            }
            return outTiles;
        }
        private Unit[,] ParceUnits(Dictionary<string, Dictionary<string, string>> ini) {
            Unit[,] units = new Unit[LengthX, LengthY];
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    units[c, r] = null;
                }
            }

            foreach (var pair in ini) {
                string section = pair.Key;
                if (section == iniSectionMap) {
                    continue;
                }

                string name = pair.Value[iniKeyUnitName];
                bool isOk = int.TryParse(pair.Value[iniKeyUnitX], out int unitX);
                isOk &= int.TryParse(pair.Value[iniKeyUnitY], out int unitY);
                isOk &= char.TryParse(pair.Value[iniKeyUnitImageChar], out char unitChar);
                if (!isOk) {
                    continue;
                }

                var unit = new Unit(name, new Point(unitX, unitY), new ConsoleImage(unitChar, ConsoleColor.Green));
            }
            return units;
        }
        public ConsoleImage[,] ToConsoleImages() {
            ConsoleImage[,] outArray = new ConsoleImage[LengthX, LengthY];
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outArray[c, r] = Units[c, r]?.ConsoleImage ?? LandTiles[c, r].ConsoleImage;
                }
            }
            outArray[SelectedTileLocation.X, SelectedTileLocation.Y].Color = selectedTileColor;
            return outArray;
        }
        public Unit GetUnit(int x, int y) => Units[x, y];
        public LandTile GetLandTile(int x, int y) => LandTiles[x, y];
        public ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandTiles[x, y].ConsoleImage;
        public ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);

    }
}
