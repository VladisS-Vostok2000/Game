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

        public LandTile[,] LandMap { get; }
        // TODO: рассмотреть возможность превратить в словари.
        public List<LandTile> LandTiles { get; }
        public Unit[,] Units { get; }
        public UnitConfigurator GlobalUnitConfigurator { get; }
        public List<Passability> Passabilities { get; }


        // Map
        private const string iniSectionMap = "Map";
        private const string iniKeyMap = "Map";
        private const string iniKeyMapLengthX = "LengthX";
        private const string iniKeyMapLengthY = "LengthY";
        // General
        private const string iniKeyDefault = "Default";
        private const string iniKeyType = "Type";
        private const string iniKeyDisplayedName = "Name";
        private const string iniKeyImageChar = "CharImage";
        // Tile
        private const string iniKeyTileColor = "Color";
        private const string iniValueTypeTile = "Tile";
        // Part
        private const string iniKeyPartMaxHP = "MaxHP";
        private const string iniKeyPartMasse = "Masse";
        // Body
        private const string iniValueTypeBody = "Body";
        // Chassis
        private const string iniValueTypeChassis = "Chassis";
        private const string iniKeyChassisPassability = "Passability";
        // Unit
        private const string iniKeyUnitX = "X";
        private const string iniKeyUnitY = "Y";
        private const string iniValueTypeUnit = "Unit";
        private const string iniKeyUnitBody = "Body";
        private const string iniKeyUnitChassis = "Chassis";
        // Passability
        private const string iniValueTypePassability = "Passability";



        public MapTileInfo this[int x, int y] => new MapTileInfo(LandMap[x, y], Units[x, y]);



        public Map(string mapPath) {
            Dictionary<string, Dictionary<string, string>> ini;
            using (var streamReader = new StreamReader(mapPath)) {
                ini = Parcer.Parse(mapPath);
            }

            LengthX = int.Parse(ini[iniSectionMap][iniKeyMapLengthX]);
            LengthY = int.Parse(ini[iniSectionMap][iniKeyMapLengthY]);
            LandTiles = ParseLandTiles(ini);
            Passabilities = ParsePassabilities(ini);
            LandMap = ParseMap(ini[iniSectionMap][iniKeyMap]);
            List<Body> bodies = ParseBodies(ini);

            List<Chassis> chasses = ParseChassis(ini);
            GlobalUnitConfigurator = new UnitConfigurator(bodies, chasses);
            Units = ParseUnits(ini);
        }



        private List<LandTile> ParseLandTiles(Dictionary<string, Dictionary<string, string>> sections) {
            var outList = new List<LandTile>();

            foreach (var pairs in sections) {
                Dictionary<string, string> section = pairs.Value;
                string sectionName = pairs.Key;
                if (!section.ContainsKeyValuePair(iniKeyType, iniValueTypeTile)) {
                    continue;
                }

                var landTile = new LandTile() { Name = sectionName };
                // Необязательные параметры.
                if (section.TryParseValue(iniKeyDisplayedName, out string name)) {
                    landTile.DisplayedName = name;
                }

                // Обязательные параметры.
                try {
                    landTile.ConsoleImage = new ConsoleImage(char.Parse(section[iniKeyImageChar]), (ConsoleColor)Enum.Parse(typeof(ConsoleColor), section[iniKeyTileColor]));
                }
                catch (FormatException) {
                    continue;
                }
                catch (KeyNotFoundException) {
                    continue;
                }

                outList.Add(landTile);
            }
            return outList;
        }
        private LandTile[,] ParseMap(string chars) {
            var outTiles = new LandTile[LengthX, LengthY];
            int i = 0;
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outTiles[c, r] = LandTile.Parse(chars[i++], LandTiles);
                }
            }
            return outTiles;
        }
        private List<Body> ParseBodies(Dictionary<string, Dictionary<string, string>> sections) {
            var outList = new List<Body>();
            foreach (var pairs in sections) {
                Dictionary<string, string> section = pairs.Value;
                string sectionName = pairs.Key;
                if (!section.ContainsKeyValuePair(iniKeyType, iniValueTypeBody)) {
                    continue;
                }

                var body = new Body() { Name = sectionName };
                InitializePart(section, body);

                outList.Add(body);
            }

            if (outList.Count == 0) {
                outList.Add(new Body() { Name = iniKeyDefault, DisplayedName = iniKeyDefault });
            }
            return outList;
        }
        // TODO: добавить (проверить) перезапись ключей.
        private List<Passability> ParsePassabilities(Dictionary<string, Dictionary<string, string>> sections) {
            var outList = new List<Passability>();
            foreach (var section in sections) {
                Dictionary<string, string> sectionPairs = section.Value;
                if (!SectionIsTypeOf(sectionPairs, iniValueTypePassability)) {
                    continue;
                }
                Dictionary<string, string> passabilitySectionPairs = sectionPairs;
                string sectionName = section.Key;

                var tilesPassability = new Dictionary<LandTile, int>();
                // Обработать нужно обязательно каждый тип LandTile.
                var landTiles = new Dictionary<string, LandTile>();
                foreach (var landTile in LandTiles) {
                    landTiles.Add(landTile.Name, landTile);
                }

                foreach (var pair in passabilitySectionPairs) {
                    try {
                        string tileName = pair.Key;
                        LandTile landTile = landTiles[tileName];
                        int tilePassabilityValue = int.Parse(pair.Value);
                        tilesPassability.Add(landTile, tilePassabilityValue);
                        // Обработан.
                        landTiles.Remove(tileName);
                    }
                    catch (FormatException) {
                        continue;
                    }
                    catch (KeyNotFoundException) {
                        continue;
                    }
                }

                foreach (var pair in landTiles) {
                    LandTile landTile = pair.Value;
                    tilesPassability.Add(landTile, Passability.MaxValue);
                }

                outList.Add(new Passability(tilesPassability));
            }
            return outList;
        }
        private List<Chassis> ParseChassis(Dictionary<string, Dictionary<string, string>> ini) {
            var outList = new List<Chassis>();

            foreach (var sections in ini) {
                var section = sections.Value;
                if (!SectionIsTypeOf(section, iniValueTypeChassis)) {
                    continue;
                }
                var chassisSection = section;
                var sectionName = sections.Key;

                var chassis = new Chassis() { Name = sectionName };
                InitializePart(chassisSection, chassis);

                // Обязательные параметры.
                try {
                    string passabilityName = chassisSection[iniKeyChassisPassability];
                    // TODO: должна ли обязательно проходимость быть внутри класса или достаточно её существования?
                    Passability passability = Passabilities.Find((Passability _passability) => _passability.Name == passabilityName) ?? throw new KeyNotFoundException();
                    chassis.Passability = passability;
                }
                catch (KeyNotFoundException) {
                    continue;
                }
                catch (FormatException) {
                    continue;
                }

                outList.Add(chassis);
            }

            if (outList.Count == 0) {
                outList.Add(new Chassis() { Name = iniKeyDefault, DisplayedName = iniKeyDefault, Passability = new Passability(LandTiles)});
            }

            return outList;
        }
        private Part InitializePart(Dictionary<string, string> section, Part part) {
            // Необязательные параметры.
            if (section.TryParseValue(iniKeyDisplayedName, out string name)) {
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
                if (section.TryParseValue(iniKeyDisplayedName, out string name)) {
                    unit.Name = name;
                }

                // Обязательные параметры.
                try {
                    unit.Location = new Point(int.Parse(section[iniKeyUnitX]), int.Parse(section[iniKeyUnitY]));
                    unit.ConsoleImage = new ConsoleImage(char.Parse(section[iniKeyImageChar]), ConsoleColor.Black);
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
        private bool SectionIsTypeOf(Dictionary<string, string> section, string type) => section.TryGetValue(iniKeyType, out string sectionType) && sectionType == type;

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
        public LandTile GetLandTile(int x, int y) => LandMap[x, y];
        public ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandMap[x, y].ConsoleImage;
        public ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);

    }
}
