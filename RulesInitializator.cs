using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Undefinded;
using System.Drawing;

namespace Game {
    /// <summary>
    /// Инкапсулирует инициализацию правил из соответствующих строк.
    /// </summary>
    public static class RulesInitializator {
        // DOING: инкапсуляция работы со строками.
        /// General
        private const string iniKeyType = "Type";
        private const string iniKeyDisplayedName = "Name";
        private const string iniKeyImageChar = "CharImage";
        private const string iniDefaultDisplayedName = "Default";
        /// Tile
        private const string iniKeyTileColor = "Color";
        private const string iniValueTypeLandtile = "Tile";
        /// Map
        private const string iniSectionMap = "Map";
        private const string iniKeyMap = "Map";
        private const string iniKeyMapLengthX = "LengthX";
        private const string iniKeyMapLengthY = "LengthY";
        /// Part
        private const string iniKeyPartMaxHP = "MaxHP";
        private const string iniKeyPartMasse = "Masse";
        private const int iniDefaultPartMaxHP = 100;
        private const int iniDefaultPartMasse = 100;
        /// Body
        private const string iniValueTypeBody = "Body";
        /// Chassis
        private const string iniValueTypeChassis = "Chassis";
        private const string iniKeyChassisPassability = "Passability";
        /// Passability
        private const string iniValueTypePassability = "Passability";
        /// Unit
        private const string iniKeyUnitDisplayedName = "Name";
        private const string iniKeyUnitX = "X";
        private const string iniKeyUnitY = "Y";
        private const string iniValueTypeUnit = "Unit";
        private const string iniKeyUnitBody = "Body";
        private const string iniKeyUnitChassis = "Chassis";
        private const string iniKeyUnitMaxHP = "MaxHP";
        private const string iniKeyUnitCurrentHP = "CurrentHP";
        private const string iniKeyUnitEngine = "Engine";
        private const string iniDefaultUnitDisplayedName = "Default";
        private const int iniDefaultUnitMaxHP = 100;
        private const int iniDefaultUnitCurrentHP = 100;
        /// Engine
        private const string iniValueTypeEngine = "Engine";
        private const string iniKeyEnginePower = "Power";



        public static Map InitializeMap(IDictionary<string, IDictionary<string, string>> rulesIni, IDictionary<string, IDictionary<string, string>> mapIni) {
            rulesIni.Remove(iniSectionMap);
            rulesIni.Remove((IDictionary<string, string> _section) => _section.TryGetValue(iniKeyType, out string type) && type == iniValueTypeUnit);

            // REFACTORING: стоит выделить отдельный словарь, ведь дальнейшее
            // использование rulesIni неожиданно?
            rulesIni.Merge(mapIni);

            List<Landtile> landTiles = ParseLandtiles(rulesIni);
            List<Body> bodies = ParseBodies(rulesIni);
            List<Passability> passabilities = ParsePassabilities(rulesIni, landTiles);
            List<Chassis> chassis = ParseChassis(rulesIni, passabilities);
            List<Engine> engines = ParseEngines(rulesIni);
            var rules = new Rules(landTiles, passabilities, bodies, chassis, engines);
            return InitializeMap(rulesIni, rules);
        }


        private static List<Landtile> ParseLandtiles(IDictionary<string, IDictionary<string, string>> sections) {
            var outList = new List<Landtile>();

            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeLandtile)) {
                    continue;
                }
                IDictionary<string, string> landtilesSectionPairs = sectionPairs;
                string landtilesSectionName = section.Key;

                // Необязательные параметры.
                string displayedName = landtilesSectionPairs.TryParseValue(iniKeyDisplayedName, out string temp) ? temp : iniDefaultDisplayedName;

                // Обязательные параметры.
                ConsoleImage consoleImage;
                try {
                    consoleImage = new ConsoleImage(char.Parse(landtilesSectionPairs[iniKeyImageChar]), (ConsoleColor)Enum.Parse(typeof(ConsoleColor), landtilesSectionPairs[iniKeyTileColor]));
                }
                catch (FormatException) {
                    continue;
                }
                catch (KeyNotFoundException) {
                    continue;
                }

                outList.Add(new Landtile(landtilesSectionName, displayedName, consoleImage));
            }
            return outList;
        }
        private static List<Passability> ParsePassabilities(IDictionary<string, IDictionary<string, string>> sections, ICollection<Landtile> landtiles) {
            var outPassabilities = new List<Passability>();
            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypePassability)) {
                    continue;
                }
                IDictionary<string, string> passabilitySectionPairs = sectionPairs;
                string passabilitySectionName = section.Key;

                var tilesPassability = new Dictionary<string, int>();
                // Обработать нужно обязательно каждый тип landtile.
                List<string> landtilesNames = GetLandtilesNames(landtiles);
                foreach (var pair in passabilitySectionPairs) {
                    try {
                        string landtileName = pair.Key;
                        int landtilePassabilityValue = int.Parse(pair.Value);
                        // Если тайл существует.
                        if (landtilesNames.Remove(landtileName)) {
                            tilesPassability.Add(landtileName, landtilePassabilityValue);
                        }
                    }
                    catch (FormatException) {
                        continue;
                    }
                }

                // Незаданные в passability landtile заполняются по умолчанию.
                foreach (var landtileName in landtilesNames) {
                    tilesPassability.Add(landtileName, Passability.MaxValue);
                }

                outPassabilities.Add(new Passability(passabilitySectionName, tilesPassability));
            }
            return outPassabilities;
        }
        private static Part ParsePart(IDictionary<string, string> sections, Part part) {
            // Необязательные параметры.
            part.DisplayedName = sections.TryParseValue(iniKeyDisplayedName, out string displayedName) ? displayedName : iniDefaultDisplayedName;
            part.MaxHP = sections.TryParseValue(iniKeyPartMaxHP, out int maxHP) ? maxHP : iniDefaultPartMaxHP;
            part.CurrentHP = part.MaxHP;
            part.Masse = sections.TryParseValue(iniKeyPartMasse, out int masse) ? masse : iniDefaultPartMasse;
            return part;
        }
        private static List<Body> ParseBodies(IDictionary<string, IDictionary<string, string>> sections) {
            var bodies = new List<Body>();

            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeBody)) {
                    continue;
                }
                IDictionary<string, string> bodySectionPairs = sectionPairs;
                string bodySectionName = section.Key;

                var body = new Body(bodySectionName);
                ParsePart(bodySectionPairs, body);

                bodies.Add(body);
            }

            return bodies;
        }
        private static List<Chassis> ParseChassis(IDictionary<string, IDictionary<string, string>> sections, ICollection<Passability> passabilities) {
            var outChassis = new List<Chassis>();

            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeChassis)) {
                    continue;
                }
                IDictionary<string, string> chassisSectionPairs = sectionPairs;
                string chassisSectionName = section.Key;

                var chassis = new Chassis(chassisSectionName);
                ParsePart(chassisSectionPairs, chassis);

                // Обязательные параметры.
                try {
                    string passabilityName = chassisSectionPairs[iniKeyChassisPassability];
                    Passability passability = passabilities.First((Passability _passability) => _passability.Name == passabilityName);
                    chassis.Passability = (Passability)passability.Clone();
                }
                catch (KeyNotFoundException) {
                    continue;
                }
                catch (FormatException) {
                    continue;
                }
                catch (InvalidOperationException) {
                    continue;
                }

                outChassis.Add(chassis);
            }

            return outChassis;
        }
        private static List<Engine> ParseEngines(IDictionary<string, IDictionary<string, string>> sections) {
            var engines = new List<Engine>();

            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeEngine)) {
                    continue;
                }
                IDictionary<string, string> engineSectionPairs = sectionPairs;
                string engineSectionName = section.Key;

                var engine = new Engine(engineSectionName);
                ParsePart(engineSectionPairs, engine);

                // Обязательные параметры.
                try {
                    engine.Power = int.Parse(engineSectionPairs[iniKeyEnginePower]);
                }
                catch (KeyNotFoundException) {
                    continue;
                }
                catch (FormatException) {
                    continue;
                }
                catch (InvalidOperationException) {
                    continue;
                }

                engines.Add(engine);
            }

            return engines;
        }
        private static Map InitializeMap(IDictionary<string, IDictionary<string, string>> ini, Rules rules) {
            foreach (var section in ini) {
                string sectionName = section.Key;
                if (sectionName != iniSectionMap) {
                    continue;
                }
                string mapSectionName = sectionName;
                IDictionary<string, string> mapSectionPairs = section.Value;

                // Обязательные параметры.
                // REFACTORING: рассмотреть возможность сократить это.
                Map map = default;
                try {
                    int x = int.Parse(mapSectionPairs[iniKeyMapLengthX]);
                    int y = int.Parse(mapSectionPairs[iniKeyMapLengthY]);
                    Landtile[,] mapLandtiles = ParseMap(mapSectionPairs[iniKeyMap], rules.Landtiles, x, y);
                    Unit[,] units = ParseUnits(ini, new Unit[x, y], rules);
                    map = new Map(mapLandtiles, rules, units);
                }
                catch (KeyNotFoundException) { }
                catch (FormatException) { }

                return map;
            }

            throw new Exception();
        }

        /// <exception cref="Exception"></exception>
        private static Landtile[,] ParseMap(string chars, ICollection<Landtile> landtiles, int x, int y) {
            var outMap = new Landtile[x, y];
            int i = 0;
            for (int r = 0; r < y; r++) {
                for (int c = 0; c < x; c++) {
                    try {
                        outMap[c, r] = landtiles.First((Landtile _landtile) => _landtile.ConsoleImage.Char == chars[i]);
                        i++;
                    }
                    catch (InvalidOperationException) {
                        throw new Exception("Неизвестный тайл.");
                    }
                }
            }
            return outMap;
        }
        private static Unit[,] ParseUnits(IDictionary<string, IDictionary<string, string>> ini, Unit[,] units, Rules rules) {
            foreach (var section in ini) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeUnit)) {
                    continue;
                }
                IDictionary<string, string> unitSectionPairs = sectionPairs;
                string unitSectionName = section.Key;

                // Необязательные параметры.
                string unitDisplayedName = unitSectionPairs.TryParseValue(iniKeyUnitDisplayedName, out string unitNameTemp) ? unitNameTemp : iniDefaultUnitDisplayedName;
                Unit unit = new Unit() { DisplayedName = unitDisplayedName };
                unit.MaxHP = unitSectionPairs.TryParseValue(iniKeyUnitMaxHP, out string unitMaxHPTemp) && int.TryParse(unitMaxHPTemp, out int unitMaxHP) ? unitMaxHP : iniDefaultUnitMaxHP;
                unit.CurrentHP = unitSectionPairs.TryParseValue(iniKeyUnitCurrentHP, out string unitCurrentHPTemp) && int.TryParse(unitCurrentHPTemp, out int unitCurrentHP) ? unitCurrentHP : iniDefaultUnitCurrentHP;

                // Обязательные параметры.
                Point unitLocation;
                try {
                    unitLocation = new Point(int.Parse(unitSectionPairs[iniKeyUnitX]), int.Parse(unitSectionPairs[iniKeyUnitY]));
                    unit.ConsoleImage = new ConsoleImage(char.Parse(unitSectionPairs[iniKeyImageChar]), ConsoleColor.Black);
                    unit.Body = rules.GetBody(unitSectionPairs[iniKeyUnitBody]);
                    unit.Chassis = rules.GetChassis(unitSectionPairs[iniKeyUnitChassis]);
                    unit.Engine = rules.GetEngine(unitSectionPairs[iniKeyUnitEngine]);
                }
                catch (FormatException) {
                    continue;
                }
                catch (KeyNotFoundException) {
                    continue;
                }
                catch (InvalidOperationException) {
                    continue;
                }

                units[unitLocation.X, unitLocation.Y] = unit;
            }

            return units;
        }

        private static bool IsSectionTypeOf(IDictionary<string, string> section, string type) => section.TryGetValue(iniKeyType, out string sectionType) && sectionType == type;
        private static List<string> GetLandtilesNames(ICollection<Landtile> landtiles) {
            var outlandtilesNames = new List<string>();
            foreach (var landtile in landtiles) {
                outlandtilesNames.Add(landtile.Name);
            }
            return outlandtilesNames;
        }

    }
}
