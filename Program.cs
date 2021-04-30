using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Undefinded;

namespace Game {
    public static class Program {
        private static string[] menuOptions = { "Начать игру", "Выйти" };
        private static ConsoleColor defaultColor = ConsoleColor.White;
        private static ConsoleColor hightlitedMenuOptionColor = ConsoleColor.Red;
        private static string rulesPath = @"rules.ini";
        private static string mapPath = @"map.txt";

        private static string buttonsInstruction = @"[→] [←] [↑] [↓] ";
        private static string buttonsEnter = "[Enter]";
        private static string buttonsSpace = "[Space]";
        private static string buttonsEscape = "[Esc]";

        private const int padConst = 15;
        private static Map map;



        public static void Main(string[] args) {
            //Console.ReadKey(true);
            Console.CursorVisible = false;
            Console.BufferHeight = 50;

            int mainMenuPointer = 0;
            PrintMainMenu(mainMenuPointer);

            do {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Enter) {
                    Console.Clear();
                    StartGame(rulesPath, mapPath);
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


        private static void WriteColored(string str, ConsoleColor color) {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = currentColor;
        }
        private static void WriteLineColored(string str, ConsoleColor color) {
            WriteColored(str, color);
            Console.WriteLine();
        }
        private static void WriteColored(char chr, ConsoleColor color) => WriteColored(chr.ToString(), color);
        private static void WriteLineColored(char chr, ConsoleColor color) => WriteColored(chr.ToString(), color);
        private static void WriteColored(ConsoleImage charImage) => WriteColored(charImage.Char, charImage.Color);
        private static void WriteLineColored(ConsoleImage charImage) => WriteLineColored(charImage.Char, charImage.Color);


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
                    else
                    if (map.SelectedTile.ContainsUnit) {
                        map.SelectUnit();
                    }
                }
                else
                if (input.Key == ConsoleKey.Spacebar) {
                    map.AddSelectedUnitRoute();
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
            } while (true);
        }
        private static Map InitializeMap(string rulesPath, string mapPath) {
            IDictionary<string, IDictionary<string, string>> rulesIni = IniParser.Parser.Parse(rulesPath);
            IDictionary<string, IDictionary<string, string>> mapIni = IniParser.Parser.Parse(mapPath);
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
            Console.WriteLine(new string('-', Console.BufferWidth - 1));
            MaptileInfo tileInfo = map.SelectedTile;
            PrintKeys(tileInfo);
            PrintTileInformation(tileInfo);
        }
        private static void PrintKeys(MaptileInfo tileInfo) {
            Console.Write(buttonsInstruction);
            if (tileInfo.ContainsUnit || map.UnitSelected) {
                Console.Write(buttonsEnter);
            }
            if (map.UnitSelected) {
                if (tileInfo.AvailableForSelectedUnitMove) {
                    Console.Write(buttonsSpace);
                }
                Console.Write(buttonsEscape);
            }
            Console.WriteLine("");
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
            Console.WriteLine("Тип кузова:".PadRight(padConst) + unit.Body.DisplayedName);
            Console.WriteLine("Тип ходовой:".PadRight(padConst) + unit.Chassis.DisplayedName);
            Console.WriteLine("Масса:".PadRight(padConst) + unit.Masse);
            Console.WriteLine("Тип двигателя:".PadRight(padConst) + unit.Engine.DisplayedName);
            Console.WriteLine("Мощность:".PadRight(padConst) + unit.Engine.Power);
        }

    }
}