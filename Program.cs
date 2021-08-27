using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using static Game.BasicTypesLibrary.ExtensionMethods.ConsoleExtensionMethods;
using static Game.ConsoleDrawingEngine.ConsoleScreen;
using static Game.ColoredCharsEngine.StaticMethods.GraphicsModificate;
using static System.Console;
using Game.ConsoleDrawingEngine;
using Game.ConsoleDrawingEngine.Controls;
using Game.ConsoleDrawingEngine.Types;
using Game.Parser;

namespace Game.Core {
    public static class Program {
        private static string[] menuOptions = { "Начать игру", "Выйти" };
        private static ConsoleColor defaultColor = ConsoleColor.White;
        private static ConsoleColor hightlitedMenuOptionColor = ConsoleColor.Red;
        private static string rulesPath = @"rules.ini";
        private static string mapPath = @"map.ini";

        private static string buttonsArrows = @"[→] [←] [↑] [↓]";
        private static string buttonEnter = "[Enter]";
        private static string buttonsSpace = "[Space]";
        private static string buttonEscape = "[Esc]";
        private static string buttonDel = "[Del]";

        private const int padConst = 15;
        private static GameMap map;



        static Program() {
            menuOptions = PadRight(menuOptions);
        }



        public static void Main(string[] args) {
            var consoleMenu = new ConsoleMenu(Point.Empty, PadRight(menuOptions));
            AddControl(consoleMenu);
            string selectedOption = ListenMenu(consoleMenu).StringOptionName;
            if (selectedOption == menuOptions[0]) {
                RemoveControl(consoleMenu);
                StartGame(rulesPath, mapPath);
            }
            else
            if (selectedOption == menuOptions[1]) {
                Environment.Exit(0);
            }
            else {
                throw new Exception();
            }
        }
        private static ConsoleMenuOption ListenMenu(ConsoleMenu menu) {
            do {
                // WORKAROUND: Render будет выполняться сам.
                Render();
                ConsoleKey key = ListenKey();
                if (key == ConsoleKey.Enter) {
                    RemoveControl(menu);
                    return menu.SelectedOption;
                }
                else {
                    if (key == ConsoleKey.DownArrow) {
                        menu.Down();
                    }
                    else
                    if (key == ConsoleKey.UpArrow) {
                        menu.Up();
                    }
                }
            } while (true);
        }


        private static void StartGame(string rulesPath, string mapPath) {
            map = InitializeMap(rulesPath, mapPath);

            AddControl(new ConsolePictureControl(Point.Empty, new ConsoleColoredCharsPicture(map.Picture)));
            Render();

            do {
                ConsoleKeyInfo input = ReadKey(true);
                MaptileInfo selectedTileInfo = map.SelectedTile;
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
                    else { map.SelectUnit(); }
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
        private static GameMap InitializeMap(string rulesPath, string mapPath) {
            IDictionary<string, IDictionary<string, string>> rulesIni = IniParser.Parse(rulesPath);
            IDictionary<string, IDictionary<string, string>> mapIni = IniParser.Parse(mapPath);
            return RulesInitializator.InitializeMap(rulesIni, mapIni);
        }
        private static void PrintMapScreen() {
            // TASK:
        }
        private static void PrintGameMenu() {
            MaptileInfo tileInfo = map.SelectedTile;
            string keys = GetStringPossibleKeys(tileInfo);
            WriteLine(keys);

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
            //WriteLineColored(currentTeam.DisplayedName, currentTeam.Color);
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
            // TODO:
        }

        private static void PrintLandtileAndUnitTitle(MaptileInfo tileInfo) {
            // TODO:
        }
        private static void PrintUnitInfo(Unit unit) {
            WriteLine("Имя:".PadRight(padConst) + unit.DisplayedName);
            WriteLine("Целостность:".PadRight(padConst) + unit.CurrentHP + "/" + unit.MaxHP);
            WriteLine("Тип кузова:".PadRight(padConst) + unit.BodyCondition.Body.DisplayedName);
            WriteLine("Тип ходовой:".PadRight(padConst) + unit.ChassisCondition.Chassis.DisplayedName);
            WriteLine("Масса:".PadRight(padConst) + unit.Masse);
            WriteLine("Тип двигателя:".PadRight(padConst) + unit.EngineCondition.Engine.DisplayedName);
            WriteLine("Мощность:".PadRight(padConst) + unit.EngineCondition.Engine.Power);
            WriteLine("Орудие:".PadRight(padConst) + unit.WeaponCondition.Weapon.DisplayedName);
            WriteLine("DebugTimeReserve:".PadRight(padConst) + unit.TimeReserve);
        }

    }
}