using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using ExtensionMethods;
using static Core.ConsoleScreen;
using Core.CharWindow;

namespace Core {
    public static class Program {
        private static string[] menuOptions = { "Начать игру", "Выйти" };
        private static ConsoleColor defaultColor = ConsoleColor.White;
        private static ConsoleColor hightlitedMenuOptionColor = ConsoleColor.Red;
        private static string rulesPath = @"rules.ini";
        private static string mapPath = @"map.txt";

        private static string buttonsArrows = @"[→] [←] [↑] [↓]";
        private static string buttonEnter = "[Enter]";
        private static string buttonsSpace = "[Space]";
        private static string buttonEscape = "[Esc]";
        private static string buttonDel = "[Del]";

        private const int padConst = 15;
        private static Map map;



        public static void Main(string[] args) {
            var field = new TextBox("а", 1, 1).CharImage;
            var field1 = new TextBox(" ", 1, 1).CharImage;
            var field2 = new TextBox("слово слово слово слово", 6, 4).CharImage;
            var field3 = new TextBox("слово  слово  слово  слово", 6, 10).CharImage;
            var field4 = new TextBox("  слово  слово  слово  слово  ", 6, 10).CharImage;
            var field5 = new TextBox("  слово  слов  сло  сл  ", 4, 10).CharImage;
            Console.ReadKey(true);
            Console.CursorVisible = false;
            Console.BufferHeight = 50;

            int mainMenuPointer = 0;
            PrintMainMenu(mainMenuPointer);

            do {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Enter) {
                    if (mainMenuPointer == 0) {
                        StartGame(rulesPath, mapPath);
                    }
                    else {
                        Environment.Exit(0);
                    }
                }
                else {
                    if (input.Key == ConsoleKey.DownArrow) {
                        mainMenuPointer = Math.Abs(++mainMenuPointer) % menuOptions.Length;
                    }
                    else
                    if (input.Key == ConsoleKey.UpArrow) {
                        mainMenuPointer = Math.Abs(--mainMenuPointer) % menuOptions.Length;
                    }
                    PrintMainMenu(mainMenuPointer);
                }
            } while (true);
        }
        private static void PrintMainMenu(int optionHighlitedNumber) {
            Console.Clear();
            for (int i = 0; i < menuOptions.Length; i++) {
                ConsoleColor consoleColor = i == optionHighlitedNumber ? hightlitedMenuOptionColor : defaultColor;
                WriteColored("* ", consoleColor);
                WriteLineColored(menuOptions[i], defaultColor);
            }
        }



        private static void StartGame(string rulesPath, string mapPath) {
            map = InitializeMap(rulesPath, mapPath);
            do {
                Console.Clear();
                PrintMapScreen();
                PrintGameMenu();
                ConsoleKeyInfo input = Console.ReadKey(true);
                MaptileInfo selectedTileInfo = map.SelectedTile;
                // Нет проверок на выделенный тайл,
                // потому что играю от API.
                if (input.Key == ConsoleKey.DownArrow) {
                    ++map.SelectedTileY;
                }
                else
                if (input.Key == ConsoleKey.UpArrow) {
                    --map.SelectedTileY;
                }
                else
                if (input.Key == ConsoleKey.LeftArrow) {
                    --map.SelectedTileX;
                }
                else
                if (input.Key == ConsoleKey.RightArrow) {
                    ++map.SelectedTileX;
                }
                else
                if (input.Key == ConsoleKey.Enter) {
                    if (map.UnitSelected) {
                        map.ConfirmSelectedUnitRoute();
                    }
                    else {map.SelectUnit(); }
                }
                else
                if (input.Key == ConsoleKey.Spacebar) {
                    map.AddSelectedUnitWay();
                }
                else
                if (input.Key == ConsoleKey.Escape) {
                    map.UnselectUnit();
                }
                else
                if (input.Key == ConsoleKey.T) {
                    map.MakeTurn();
                }
                else
                if (input.Key == ConsoleKey.P) {
                    map.PassTurn();
                }
                else
                if (input.Key == ConsoleKey.Delete) {
                    if (selectedTileInfo.SelectedUnitWay) {
                        map.DeleteSelectedUnitLastWay();
                    }
                }
            } while (true);
        }
        private static Map InitializeMap(string rulesPath, string mapPath) {
            IDictionary<string, IDictionary<string, string>> rulesIni = Parser.IniParser.Parse(rulesPath);
            IDictionary<string, IDictionary<string, string>> mapIni = Parser.IniParser.Parse(mapPath);
            return RulesInitializator.InitializeMap(rulesIni, mapIni);
        }
        private static void PrintMapScreen() {
            ConsoleImage[,] images = map.Visualize();
            for (int r = 0; r < map.LengthY; r++) {
                for (int c = 0; c < map.LengthX; c++) {
                    WriteColored(images[c, r]);
                }
                Console.WriteLine();
            }
        }
        private static void PrintGameMenu() {
            WriteSeparator();
            MaptileInfo tileInfo = map.SelectedTile;
            string keys = GetStringPossibleKeys(tileInfo);
            Console.WriteLine(keys);

            WriteSeparator();
            PrintCurrentTeamInfo(map.CurrentTeam);
            PrintTileInformation(tileInfo);
        }


        private static string GetStringPossibleKeys(MaptileInfo maptileInfo) {
            string outString = buttonsArrows;
            if (map.UnitSelected) {
                outString += " " + buttonEnter;
                if (maptileInfo.AvailableForSelectedUnitMove) { outString += " " + buttonsSpace; }

                if (maptileInfo.SelectedUnitWay) { outString += " " + buttonDel; }
                outString += " " + buttonEscape;
            }
            else {
                if (maptileInfo.ContainsUnit) { outString += " " + buttonEnter; }
            }
            return outString;
        }
        private static void PrintCurrentTeamInfo(Team currentTeam) {
            WriteLineColored(currentTeam.DisplayedName, currentTeam.Color);
        }

        private static void PrintTileInformation(MaptileInfo tileInfo) {
            if (tileInfo.ContainsUnit) {
                PrintLandtileAndUnitTitle(tileInfo);
            }
            else {
                PrintLandtileTitle(tileInfo);
            }

            if (tileInfo.ContainsUnit) {
                PrintUnitInfo(tileInfo.Unit);
            }
        }
        private static void PrintLandtileTitle(MaptileInfo tileInfo) {
            Landtile landtile = tileInfo.Land;
            Console.Write("[");
            WriteColored(landtile.ConsoleImage);
            Console.Write("] ");

            string name = tileInfo.Land.DisplayedName;
            Console.WriteLine(name + $"({map.SelectedTileX}; {map.SelectedTileY})");
        }

    }
}
        private static void PrintLandtileAndUnitTitle(MaptileInfo tileInfo) {
            Landtile landtile = tileInfo.Land;
            Console.Write("[");
            WriteColored(landtile.ConsoleImage);
            Console.Write("]");
            Unit unit = tileInfo.Unit;
            Console.Write("[");
            WriteColored(tileInfo.Unit.ConsoleImage);
            Console.Write("]");
            string landtileName = tileInfo.Land.DisplayedName;
            string unitName = tileInfo.Unit.DisplayedName;
            Console.WriteLine(" " + landtileName + "/" + unitName + $"({map.SelectedTileX}; {map.SelectedTileY})");
        }
        private static void PrintUnitInfo(Unit unit) {
            Console.WriteLine("Имя:".PadRight(padConst) + unit.DisplayedName);
            Console.WriteLine("Целостность:".PadRight(padConst) + unit.CurrentHP + "/" + unit.MaxHP);
            Console.WriteLine("Тип кузова:".PadRight(padConst) + unit.BodyCondition.Body.DisplayedName);
            Console.WriteLine("Тип ходовой:".PadRight(padConst) + unit.ChassisCondition.Chassis.DisplayedName);
            Console.WriteLine("Масса:".PadRight(padConst) + unit.Masse);
            Console.WriteLine("Тип двигателя:".PadRight(padConst) + unit.EngineCondition.Engine.DisplayedName);
            Console.WriteLine("Мощность:".PadRight(padConst) + unit.EngineCondition.Engine.Power);
            Console.WriteLine("Орудие:".PadRight(padConst) + unit.WeaponCondition.Weapon.DisplayedName);
            Console.WriteLine("DebugTimeReserve:".PadRight(padConst) + unit.TimeReserve);
        }