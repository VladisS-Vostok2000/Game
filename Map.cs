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
        public UnitConfigurator GlobalUnitConfigurator { get; }


        // Map
        private const string iniSectionMap = "Map";
        private const string iniKeyMap = "Map";
        private const string iniKeyMapLengthX = "LengthX";
        private const string iniKeyMapLengthY = "LengthY";
        // General
        private const string iniKeyType = "Type";
        // Unit
        private const string iniKeyUnitName = "Name";
        private const string iniKeyUnitX = "X";
        private const string iniKeyUnitY = "Y";
        private const string iniKeyUnitImageChar = "CharImage";
        private const string iniValueTypeUnit = "Unit";
        private const string iniKeyUnitBody = "Body";
        private const string iniKeyUnitChassis = "Chassis";
        // Part
        private const string iniKeyPartMaxHP = "MaxHP";
        private const string iniKeyPartDisplayedName = "Name";
        private const string iniKeyPartMasse = "Masse";
        // Body
        private const string iniValueTypeBody = "Body";
        // Chassis
        private const string iniValueTypeChassis = "Chassis";
        private const string iniKeyChassisPassability = "Passability";



        public MapTileInfo this[int x, int y] => new MapTileInfo(LandTiles[x, y], Units[x, y]);



        public Map(string mapPath) {
            Dictionary<string, Dictionary<string, string>> ini;
            using (var streamReader = new StreamReader(mapPath)) {
                ini = Parcer.Parse(mapPath);
            }

            LengthX = int.Parse(ini[iniSectionMap][iniKeyMapLengthX]);
            LengthY = int.Parse(ini[iniSectionMap][iniKeyMapLengthY]);

            LandTiles = ParseLandTiles(ini[iniSectionMap][iniKeyMap]);
            List<Body> bodies = ParseBodies(ini);
            List<Chassis> chasses = ParseChassis(ini);
            GlobalUnitConfigurator = new UnitConfigurator(bodies, chasses);
            Units = ParseUnits(ini);
        }



        private LandTile[,] ParseLandTiles(string chars) {
            var outTiles = new LandTile[LengthX, LengthY];
            int i = 0;
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    LandTile.LandTileTypes landTileType = LandTile.CharToLandTileType(chars[i++]);
                    outTiles[c, r] = landTileType != LandTile.LandTileTypes.None ? new LandTile(landTileType) : throw new Exception();
                }
            }
            return outTiles;
        }
        private List<Body> ParseBodies(Dictionary<string, Dictionary<string, string>> ini) {
            var outList = new List<Body>();
            foreach (var pair in ini) {
                Dictionary<string, string> section = pair.Value;
                string sectionName = pair.Key;
                if (!section.ContainsKeyValuePair(iniKeyType, iniValueTypeBody)) {
                    continue;
                }

                var body = new Body() { Name = sectionName };
                InitializePart(section, body);

                outList.Add(body);
            }

            if (outList.Count == 0) {
                outList.Add(new Body() { Name = "Default", DisplayedName = "Default" });
            }
            return outList;
        }
        private List<Chassis> ParseChassis(Dictionary<string, Dictionary<string, string>> ini) {
            var outList = new List<Chassis>();

            foreach (var pair in ini) {
                var sectionName = pair.Key;
                var section = pair.Value;
                if (!section.ContainsKeyValuePair(iniKeyType, iniValueTypeChassis)) {
                    continue;
                }

                var chassis = new Chassis() { Name = sectionName };
                InitializePart(section, chassis);
                // Необязательные параметры.
                if (section.TryParseValue(iniKeyChassisPassability, out int passability)) {
                    chassis.Passability = passability;
                }

                outList.Add(chassis);
            }

            if (outList.Count == 0) {
                outList.Add(new Chassis() { Name = "Default", DisplayedName = "Default" });
            }

            return outList;
        }
        private Part InitializePart(Dictionary<string, string> section, Part part) {
            // Необязательные параметры.
            if (section.TryParseValue(iniKeyPartDisplayedName, out string name)) {
                part.DisplayedName = name;
            }
            if (section.TryParseValue(iniKeyPartMaxHP, out int maxHP)) {
                part.MaxHP = maxHP;
            }
            part.CurrentHP = part.MaxHP;
            if (section.TryParseValue(iniKeyPartMasse, out int masse)) {
                part.Masse = masse;
            }
            return part;
        }
        private Unit[,] ParseUnits(Dictionary<string, Dictionary<string, string>> ini) {
            var units = new Unit[LengthX, LengthY];

            foreach (var pairs in ini) {
                Dictionary<string, string> section = pairs.Value;
                if (!section.ContainsKeyValuePair(iniKeyType, iniValueTypeUnit)) {
                    continue;
                }
                string sectionName = pairs.Key;

                var unit = new Unit();
                // Не обязательные параметры.
                if (section.TryParseValue(iniKeyUnitName, out string name)) {
                    unit.Name = name;
                }

                // Обязательные параметры.
                try {
                    unit.Location = new Point(int.Parse(section[iniKeyUnitX]), int.Parse(section[iniKeyUnitY]));
                    unit.ConsoleImage = new ConsoleImage(char.Parse(section[iniKeyUnitImageChar]), ConsoleColor.Black);
                    unit.SetBody(section[iniKeyUnitBody], GlobalUnitConfigurator);
                    unit.SetChassis(section[iniKeyUnitChassis], GlobalUnitConfigurator);
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
