using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undefinded;

namespace Game {
    public sealed class Rules {
        // TODO: рассмотреть возможность превратить в словари (классы?).
        private ICollection<Landtile> landtiles;
        private UnitConfigurator UnitConfigurator { get; }


        // TODO: дублирующиеся константы: рассмотреть возможность передавать секцию,
        // а не ini, что
        // позволит избавиться от дублирующейся секции ключей ниже
        /// General
        private const string iniKeyType = "Type";
        private const string iniKeyDisplayedName = "Name";
        private const string iniKeyImageChar = "CharImage";
        private const string iniDefaultDisplayedName = "Default";
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
        /// Tile
        private const string iniKeyTileColor = "Color";
        private const string iniValueTypeTile = "Tile";



        public Rules(Dictionary<string, Dictionary<string, string>> ini) {
            Landtiles = Parselandtiles(ini);
            UnitConfigurator = new UnitConfigurator(ParseBodies(ini), ParseChassis(ini, ParsePassabilities(ini, GetLandtiles)));
        }



        public Landtile ParseLandtile(char chr) => landtiles.First((Landtile landtile) => landtile.ConsoleImage.Char == chr);


        private List<Landtile> Parselandtiles(Dictionary<string, Dictionary<string, string>> sections) {
            var outList = new List<Landtile>();

            foreach (var section in sections) {
                Dictionary<string, string> sectionPairs = section.Value;
                // TODO: рассмотреть возможность заключить в метод IsValidSection(SectionType)
                if (!sectionPairs.ContainsKeyValuePair(iniKeyType, iniValueTypeTile)) {
                    continue;
                }
                Dictionary<string, string> landtilesSectionPairs = sectionPairs;
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
        private Part ParsePart(Dictionary<string, string> sections, Part part) {
            // Необязательные параметры.
            part.DisplayedName = sections.TryParseValue(iniKeyDisplayedName, out string displayedName) ? displayedName : iniDefaultDisplayedName;
            part.MaxHP = sections.TryParseValue(iniKeyPartMaxHP, out int maxHP) ? maxHP : iniDefaultPartMaxHP;
            part.CurrentHP = part.MaxHP;
            part.Masse = sections.TryParseValue(iniKeyPartMasse, out int masse) ? masse : iniDefaultPartMasse;
            return part;
        }
        private List<Body> ParseBodies(Dictionary<string, Dictionary<string, string>> sections) {
            var bodies = new List<Body>();

            foreach (var section in sections) {
                Dictionary<string, string> sectionPairs = section.Value;
                if (!sectionPairs.ContainsKeyValuePair(iniKeyType, iniValueTypeBody)) {
                    continue;
                }
                Dictionary<string, string> bodySectionPairs = section.Value;
                string bodySectionName = section.Key;

                var body = new Body(bodySectionName);
                ParsePart(bodySectionPairs, body);

                bodies.Add(body);
            }

            if (bodies.Count == 0) {
                throw new Exception();
            }
            return bodies;
        }
        private List<Passability> ParsePassabilities(Dictionary<string, Dictionary<string, string>> sections, ICollection<Landtile> landtiles) {
            var outPassabilities = new List<Passability>();
            foreach (var section in sections) {
                Dictionary<string, string> sectionPairs = section.Value;
                if (!SectionIsTypeOf(sectionPairs, iniValueTypePassability)) {
                    continue;
                }
                Dictionary<string, string> passabilitySectionPairs = sectionPairs;
                string passabilitySectionName = section.Key;

                var tilesPassability = new Dictionary<string, int>();
                // Обработать нужно обязательно каждый тип landtile.
                List<string> landtilesNames = GetlandtilesNames(landtiles);

                foreach (var pair in passabilitySectionPairs) {
                    try {
                        string landtileName = pair.Key;
                        int tilePassabilityValue = int.Parse(pair.Value);
                        // Если тайл существующий.
                        if (landtilesNames.Remove(landtileName)) {
                            tilesPassability.Add(landtileName, tilePassabilityValue);
                        }
                    }
                    catch (FormatException) {
                        continue;
                    }
                    catch (KeyNotFoundException) {
                        continue;
                    }
                }

                foreach (var landtileName in landtilesNames) {
                    tilesPassability.Add(landtileName, Passability.MaxValue);
                }

                outPassabilities.Add(new Passability(passabilitySectionName, tilesPassability));
            }
            return outPassabilities;
        }
        private List<string> GetlandtilesNames(ICollection<Landtile> landtiles) {
            var outlandtilesNames = new List<string>();
            foreach (var landtile in landtiles) {
                outlandtilesNames.Add(landtile.Name);
            }
            return outlandtilesNames;
        }
        private List<Chassis> ParseChassis(Dictionary<string, Dictionary<string, string>> sections, ICollection<Passability> passabilities) {
            var outChassis = new List<Chassis>();

            foreach (var section in sections) {
                var sectionPairs = section.Value;
                if (!SectionIsTypeOf(sectionPairs, iniValueTypeChassis)) {
                    continue;
                }
                Dictionary<string, string> chassisSectionPairs = sectionPairs;
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

                outChassis.Add(chassis);
            }

            return outChassis;
        }


        private bool SectionIsTypeOf(Dictionary<string, string> section, string type) => section.TryGetValue(iniKeyType, out string sectionType) && sectionType == type;

    }
}
