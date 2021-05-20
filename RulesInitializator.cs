using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Undefinded;
using System.Drawing;
using MyParsers;

namespace Game {
    /// <summary>
    /// Инкапсулирует инициализацию правил из соответствующих строк.
    /// </summary>
    public static class RulesInitializator {
        /// General
        private const string iniKeyType = "Type";
        private const string iniKeyDisplayedName = "Name";
        private const string iniKeyUnitImageChar = "CharImage";
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
        /// Engine
        private const string iniValueTypeEngine = "Engine";
        private const string iniKeyEnginePower = "Power";
        /// Warhead
        private const string iniValueTypeWarhead = "Warhead";
        private const string iniKeyWarheadDamage = "Damage";
        /// Projectiles
        private const string iniValueTypeProjectile = "Projectile";
        private const string iniKeyProjectileWarhead = "Warhead";
        /// Weapon
        private const string iniValueTypeWeapon = "Weapon";
        private const string iniKeyWeaponCooldown = "Cooldown";
        private const string iniKeyWeaponName = "Name";
        private const string iniKeyWeaponPojectile = "Projectile";
        private const float iniDefaultWeaponCooldown = 5;
        /// Route
        private const string iniValueTypeRoute = "Route";
        private const string iniKeyRoute = "Route";
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
        private const string iniKeyUnitTeam = "Team";
        private const string iniKeyUnitWeapon = "Weapon";
        private const string iniKeyUnitColor = "Color";
        private const string iniKeyUnitRoute = "Route";
        private const int iniDefaultUnitMaxHP = 100;
        private const int iniDefaultUnitCurrentHP = 100;
        private const string iniDefaultUnitDisplayedName = "Default";
        private const ConsoleColor iniDefaultUnitColor = ConsoleColor.Black;
        /// Team
        private const string iniValueTypeTeam = "Team";
        private const string iniKeyTeamDisplayedName = "Name";
        private const string iniKeyTeamColor = "Color";
        /// Tile
        private const string iniKeyTileColor = "Color";
        private const string iniValueTypeLandtile = "Tile";
        /// Map
        private const string iniSectionMap = "Map";
        private const string iniKeyMap = "Map";
        private const string iniKeyMapLengthX = "LengthX";
        private const string iniKeyMapLengthY = "LengthY";



        public static Map InitializeMap(IDictionary<string, IDictionary<string, string>> rulesIni, IDictionary<string, IDictionary<string, string>> mapIni) {
            rulesIni.Remove(iniSectionMap);
            rulesIni.Remove((IDictionary<string, string> _section) => _section.TryGetValue(iniKeyType, out string type) && type == iniValueTypeUnit);

            // REFACTORING: стоит выделить отдельный словарь, ведь дальнейшее
            // использование rulesIni неожиданно?
            rulesIni.Merge(mapIni);

            var rules = new Rules();
            rules.Landtiles = ParseLandtiles(rulesIni);
            rules.Passabilities = ParsePassabilities(rulesIni, rules);
            rules.Chassis = ParseChassis(rulesIni, rules);
            rules.Bodies = ParseBodies(rulesIni);
            rules.Engines = ParseEngines(rulesIni);
            rules.Warheads = ParseWarheads(rulesIni);
            rules.Projectiles = ParseProjectiles(rulesIni, rules);
            rules.Weapons = ParseWeapons(rulesIni, rules);
            rules.Teams = ParseTeams(rulesIni);
            rules.Routes = ParseRoutes(rulesIni);
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
                    consoleImage = new ConsoleImage(char.Parse(landtilesSectionPairs[iniKeyUnitImageChar]), (ConsoleColor)Enum.Parse(typeof(ConsoleColor), landtilesSectionPairs[iniKeyTileColor]));
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
        private static List<Passability> ParsePassabilities(IDictionary<string, IDictionary<string, string>> sections, Rules rules) {
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
                List<string> landtilesNames = GetLandtilesNames(rules.Landtiles);
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
        private static List<Chassis> ParseChassis(IDictionary<string, IDictionary<string, string>> sections, Rules rules) {
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
                    Passability passability = rules.GetPassability(iniKeyChassisPassability);
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
        private static List<Team> ParseTeams(IDictionary<string, IDictionary<string, string>> sections) {
            var teams = new List<Team>();

            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeTeam)) continue;

                IDictionary<string, string> teamSectionPairs = sectionPairs;
                string teamSectionName = section.Key;
                var team = new Team(teamSectionName);

                try {
                    bool parsedSusseffully = Enum.TryParse(teamSectionPairs[iniKeyTeamColor], out ConsoleColor parsedColor);
                    if (!parsedSusseffully) continue;

                    team.Color = parsedColor;
                    team.DisplayedName = teamSectionPairs[iniKeyTeamDisplayedName];
                }
                catch (KeyNotFoundException) {
                    throw;
                }

                teams.Add(team);
            }

            return teams;
        }
        private static List<Warhead> ParseWarheads(IDictionary<string, IDictionary<string, string>> sections) {
            var outList = new List<Warhead>();
            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeWarhead)) continue;

                IDictionary<string, string> warheadSectionPairs = sectionPairs;
                string warheadSectionName = section.Key;
                var warhead = new Warhead(warheadSectionName) { };

                try {
                    warhead.Damage = int.Parse(warheadSectionPairs[iniKeyWarheadDamage]);
                }
                catch (FormatException) { continue; }
                catch (KeyNotFoundException) { continue; }

                outList.Add(warhead);
            }
            return outList;
        }
        private static List<Projectile> ParseProjectiles(IDictionary<string, IDictionary<string, string>> sections, Rules rules) {
            var outList = new List<Projectile>();
            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeProjectile)) continue;

                IDictionary<string, string> projectileSectionPairs = sectionPairs;
                string projectileSectionName = section.Key;
                var projectile = new Projectile(projectileSectionName) { };

                try {
                    string warheadName = projectileSectionPairs[iniKeyProjectileWarhead];
                    projectile.Warhead = rules.GetWarhead(warheadName);
                }
                catch (KeyNotFoundException) { continue; }
                catch (InvalidOperationException) { continue; }

                outList.Add(projectile);
            }
            return outList;
        }
        private static List<Weapon> ParseWeapons(IDictionary<string, IDictionary<string, string>> sections, Rules rules) {
            var outList = new List<Weapon>();
            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeWeapon)) continue;

                IDictionary<string, string> weaponSectionPairs = sectionPairs;
                string weaponSectionName = section.Key;
                var weapon = new Weapon(weaponSectionName) {
                    // Необязательные параметры.
                    Cooldown = weaponSectionPairs.TryParseValue(iniKeyWeaponCooldown, out float parsedCooldown) ? parsedCooldown : iniDefaultWeaponCooldown,
                    DisplayedName = weaponSectionPairs.TryParseValue(iniKeyWeaponName, out string parsedName) ? parsedName : iniDefaultDisplayedName
                };

                try {
                    weapon.Projectile = rules.GetProjectile(iniKeyWeaponPojectile);
                }
                catch (KeyNotFoundException) { continue; }
                catch (InvalidOperationException) { continue; }

                outList.Add(weapon);
            }

            return outList;
        }
        private static List<NamedRoute> ParseRoutes(IDictionary<string, IDictionary<string, string>> sections) {
            var outList = new List<NamedRoute>();
            foreach (var section in sections) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeRoute)) continue;

                IDictionary<string, string> routeSectionPairs = sectionPairs;
                string routeSectionName = section.Key;
                NamedRoute namedRoute;

                try {
                    var points = PointListParser.ParsePouintList(routeSectionPairs[iniKeyRoute]);
                    var route = new Route(points);
                    namedRoute = new NamedRoute(routeSectionName, route);
                }
                catch (KeyNotFoundException) { continue; }
                catch (InvalidOperationException) { continue; }
                // REFACTORING: perfect solution
                catch (Exception) { continue; }

                outList.Add(namedRoute);
            }

            return outList;
        }

        private static Landtile[,] ParseMap(string chars, ICollection<Landtile> landtiles, int x, int y) {
            var outMap = new Landtile[x, y];
            int i = 0;
            for (int r = 0; r < y; r++) {
                for (int c = 0; c < x; c++) {
                    outMap[c, r] = landtiles.First((Landtile _landtile) => _landtile.ConsoleImage.Char == chars[i]);
                    i++;
                }
            }
            return outMap;
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
                    List<Unit> units = ParseUnits(ini, rules);
                    map = new Map(mapLandtiles, rules, units);
                }
                catch (KeyNotFoundException) { }
                catch (FormatException) { }

                return map;
            }

            throw new Exception();
        }
        private static List<Unit> ParseUnits(IDictionary<string, IDictionary<string, string>> ini, Rules rules) {
            var outList = new List<Unit>();
            foreach (var section in ini) {
                IDictionary<string, string> sectionPairs = section.Value;
                if (!IsSectionTypeOf(sectionPairs, iniValueTypeUnit)) { continue; }
                IDictionary<string, string> unitSectionPairs = sectionPairs;
                string unitSectionName = section.Key;

                // Необязательные параметры.
                string displayedname = unitSectionPairs.TryParseValue(iniKeyUnitDisplayedName, out string unitNameTemp) ? unitNameTemp : iniDefaultUnitDisplayedName;
                int maxhp = unitSectionPairs.TryParseValue(iniKeyUnitMaxHP, out string parsedUnitMaxHP) && int.TryParse(parsedUnitMaxHP, out int unitMaxHP) ? unitMaxHP : iniDefaultUnitMaxHP;
                int currenthp = unitSectionPairs.TryParseValue(iniKeyUnitCurrentHP, out string parsedUnitCurrentHP) && int.TryParse(parsedUnitCurrentHP, out int unitCurrentHP) ? unitCurrentHP : iniDefaultUnitCurrentHP;
                ConsoleColor color = unitSectionPairs.TryParseValue(iniKeyUnitColor, out string parsedUnitColor) && Enum.TryParse(parsedUnitColor, out ConsoleColor parsedColor) ? parsedColor : iniDefaultUnitColor;
                Route route = unitSectionPairs.TryParseValue(iniKeyUnitRoute, out string extractedUnitRouteName) ? rules.GetNamedRoute(extractedUnitRouteName).Route : null;

                // Обязательные параметры.
                try {
                    Point location = new Point(int.Parse(unitSectionPairs[iniKeyUnitX]), int.Parse(unitSectionPairs[iniKeyUnitY]));
                    char consolechar = char.Parse(unitSectionPairs[iniKeyUnitImageChar]);
                    BodyCondition unitBody = new BodyCondition(rules.GetBody(unitSectionPairs[iniKeyUnitBody]));
                    ChassisCondition unitChassis = new ChassisCondition(rules.GetChassis(unitSectionPairs[iniKeyUnitChassis]));
                    EngineCondition unitEngine = new EngineCondition(rules.GetEngine(unitSectionPairs[iniKeyUnitEngine]));
                    WeaponCondition unitWeapon = new WeaponCondition(rules.GetWeapon(unitSectionPairs[iniKeyUnitWeapon]));
                    Team team = rules.GetTeam(unitSectionPairs[iniKeyUnitTeam]);
                    Unit unit = new Unit(location, unitBody, unitChassis, unitEngine, unitWeapon);
                    unit.ConsoleChar = consolechar;
                    unit.Team = team;
                    unit.DisplayedName = displayedname;
                    unit.MaxHP = maxhp;
                    unit.CurrentHP = currenthp;
                    unit.Color = color;
                    if (route != null) {
                        unit.TrySetRoute(route);
                    }
                    outList.Add(unit);
                }
                catch (FormatException) { continue; }
                catch (KeyNotFoundException) { continue; }
                catch (InvalidOperationException) { continue; }
            }

            return outList;
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
