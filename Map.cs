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
        public int LengthX { get; }
        public int LengthY { get; }
        public int Square => LengthX * LengthY;
        public Unit[,] Units { get; }


        /// Map
        private const string iniSectionMap = "Map";
        private const string iniKeyMap = "Map";
        private const string iniKeyMapLengthX = "LengthX";
        private const string iniKeyMapLengthY = "LengthY";
        /// Unit
        private const string iniKeyType = "Type";
        private const string iniKeyUnitDisplayedName = "Name";
        private const string iniKeyUnitX = "X";
        private const string iniKeyUnitY = "Y";
        private const string iniValueTypeUnit = "Unit";
        private const string iniKeyUnitBody = "Body";
        private const string iniKeyUnitChassis = "Chassis";
        private const string iniDefaultUnitDisplayedName = "Default";


        public Landtile[,] LandMap { get; }
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



        public MapTileInfo this[int x, int y] => new MapTileInfo(LandMap[x, y], Units[x, y]);



        public Map(string mapPath, string rulesPath) {
            Dictionary<string, Dictionary<string, string>> rulesIni = Parser.Parse(rulesPath);
            Dictionary<string, Dictionary<string, string>> mapIni = Parser.Parse(mapPath);
            rulesIni.Merge(mapIni);

            Rules mapRulesRedaction = new Rules(rulesIni);

            LengthX = int.Parse(rulesIni[iniSectionMap][iniKeyMapLengthX]);
            LengthY = int.Parse(rulesIni[iniSectionMap][iniKeyMapLengthY]);

            LandMap = ParseMap(rulesIni[iniSectionMap][iniKeyMap], mapRulesRedaction);
            Units = ParseUnits(rulesIni, mapRulesRedaction);
        }



        private Landtile[,] ParseMap(string chars, Rules rules) {
            var outMap = new Landtile[LengthX, LengthY];
            int i = 0;
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outMap[c, r] = rules.ParseLandtile(chars[i++]);
                }
            }
            return outMap;
        }
        private Unit[,] ParseUnits(Dictionary<string, Dictionary<string, string>> ini, Rules rules) {
            var units = new Unit[LengthX, LengthY];

            foreach (var section in ini) {
                Dictionary<string, string> sectionPairs = section.Value;
                if (!sectionPairs.ContainsKeyValuePair(iniKeyType, iniValueTypeUnit)) {
                    continue;
                }
                Dictionary<string, string> unitSectionPairs = sectionPairs;
                string unitSectionName = section.Key;

                // Не обязательные параметры.
                string unitName = unitSectionPairs.TryParseValue(iniKeyUnitDisplayedName, out string unitNameTemp) ? unitNameTemp : iniDefaultUnitDisplayedName;

                // Обязательные параметры.
                try {
                    unit.Location = new Point(int.Parse(unitSectionPairs[iniKeyUnitX]), int.Parse(unitSectionPairs[iniKeyUnitY]));
                    unit.ConsoleImage = new ConsoleImage(char.Parse(unitSectionPairs[iniKeyImageChar]), ConsoleColor.Black);
                    unit.SetBody(unitSectionPairs[iniKeyUnitBody], GlobalUnitConfigurator);
                    unit.SetChassis(unitSectionPairs[iniKeyUnitChassis], GlobalUnitConfigurator);
                }
                catch (FormatException) {
                    continue;
                }
                catch (KeyNotFoundException) {
                    continue;
                }

                units[unit.Location.X, unit.Location.Y] = unit;
            }

            return units;
        }



        public ConsoleImage[,] ToConsoleImages() {
            ConsoleImage[,] outArray = new ConsoleImage[LengthX, LengthY];
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outArray[c, r] = Units[c, r]?.ConsoleImage ?? LandMap[c, r].ConsoleImage;
                }
            }
            outArray[SelectedTileLocation.X, SelectedTileLocation.Y].Color = selectedTileColor;
            return outArray;
        }
        public Unit GetUnit(int x, int y) => Units[x, y];
        public Landtile Getlandtile(int x, int y) => LandMap[x, y];
        public ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandMap[x, y].ConsoleImage;
        public ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);

    }
}
