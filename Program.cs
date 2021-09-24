using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using static Game.BasicTypesLibrary.Extensions.ConsoleExtensionMethods;
using static Game.ConsoleControlsEngine.ConsoleScreen;
using static Game.ColoredCharsEngine.StaticMethods.GraphicsModificate;
using static System.Console;
using Game.ConsoleControlsEngine;
using Game.ConsoleControlsEngine.Controls;
using Game.ConsoleControlsEngine.Types;
using Game.Parser;
using Game.ColoredCharsEngine;
using Game.BasicTypesLibrary.Extensions;
using static Game.ColoredCharsEngine.TypesExtensions;

namespace Game.Core {
    public static class Program {
        private static string[] menuOptions = { "Начать игру", "Выйти" };
        private static string rulesPath = @"rules.ini";
        private static string mapPath = @"map.ini";

        private static string buttonsArrows = @"[→] [←] [↑] [↓]";
        private static string buttonEnter = "[Enter]";
        private static string buttonsSpace = "[Space]";
        private static string buttonEscape = "[Esc]";
        private static string buttonDel = "[Del]";

        private const int padConst = 15;
        private static GameMap gameMap;



        static Program() {
            menuOptions = PadRight(menuOptions);
        }



        public static void Main(string[] args) {
            var consoleMenu = new ConsoleMenuControl(Point.Empty, menuOptions.ToMulticoloredStringsEnum());
            AddControl(consoleMenu);
            string selectedOption = ListenMenu(consoleMenu).Text;
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
        private static ConsoleMenuOption ListenMenu(ConsoleMenuControl menu) {
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
            gameMap = InitializeMap(rulesPath, mapPath);

            AddControl(new ConsolePictureControl(Point.Empty, new ConsoleColoredCharsPicture(gameMap.Picture)));
            Render();

            do {
                ConsoleKeyInfo input = ReadKey(true);
                MaptileInfo selectedTileInfo = gameMap.SelectedTile;
                if (input.Key == ConsoleKey.DownArrow) {
                    ++gameMap.SelectedTileY;
                }
                else
                if (input.Key == ConsoleKey.UpArrow) {
                    --gameMap.SelectedTileY;
                }
                else
                if (input.Key == ConsoleKey.LeftArrow) {
                    --gameMap.SelectedTileX;
                }
                else
                if (input.Key == ConsoleKey.RightArrow) {
                    ++gameMap.SelectedTileX;
                }
                else
                if (input.Key == ConsoleKey.Enter) {
                    if (gameMap.UnitSelected) {
                        gameMap.ConfirmSelectedUnitRoute();
                    }
                    else {
                        gameMap.SelectUnit();
                    }
                }
                else
                if (input.Key == ConsoleKey.Spacebar) {
                    gameMap.AddSelectedUnitWay();
                }
                else
                if (input.Key == ConsoleKey.Escape) {
                    gameMap.UnselectUnit();
                }
                else
                if (input.Key == ConsoleKey.T) {
                    gameMap.MakeTurn();
                }
                else
                if (input.Key == ConsoleKey.P) {
                    gameMap.ChangeTeam();
                }
                else
                if (input.Key == ConsoleKey.Delete) {
                    if (selectedTileInfo.SelectedUnitWay) {
                        gameMap.DeleteSelectedUnitLastWay();
                    }
                }

                CursorPosition = new Point(25, 25);
                // TEMP:
                WriteLine(gameMap.CurrentTeam.DisplayedName);

                gameMap.HardRender();
                Render();
            } while (true);
        }
        private static GameMap InitializeMap(string rulesPath, string mapPath) {
            IDictionary<string, IDictionary<string, string>> rulesIni = IniParser.Parse(rulesPath);
            IDictionary<string, IDictionary<string, string>> mapIni = IniParser.Parse(mapPath);
            return RulesInitializator.InitializeMap(rulesIni, mapIni);
        }


        private static string GetStringPossibleKeys(MaptileInfo maptileInfo) {
            string outString = buttonsArrows;
            if (gameMap.UnitSelected) {
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