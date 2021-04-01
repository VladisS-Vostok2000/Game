using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Game {
    internal static class Program {
        private static string[] menuOptions = { "Начать игру", "Выйти" };
        private static ConsoleColor defaultColor = ConsoleColor.White;
        private static ConsoleColor hightlitedMenuOptionColor = ConsoleColor.Red;
        private static ConsoleColor hightlitedMapTileColor = ConsoleColor.Yellow;
        private static string mapPath = @"map.txt";
        private static string buttonsInstruction = @"[→] [←] [↑] [↓]";

        private static int mapCursorPositionX;
        private static int mapCursorPositionY;
        private static int MapCursorPositionX {
            get => mapCursorPositionX;
            set {
                // Экранируем отрицательные числа; движение зацикленное.
                mapCursorPositionX = value % map.LengthX;
                if (mapCursorPositionX < 0) {
                    mapCursorPositionX += map.LengthX;
                }
            }
        }
        private static int MapCursorPositionY {
            get => mapCursorPositionY;
            set {
                // Экранируем отрицательные числа; движение зацикленное.
                mapCursorPositionY = value % map.LengthY;
                if (mapCursorPositionY < 0) {
                    mapCursorPositionY += map.LengthY;
                }
            }
        }
        private static Point MapCursorPosition {
            get => new Point(MapCursorPositionX, MapCursorPositionY);
            set {
                MapCursorPositionX = value.X;
                MapCursorPositionY = value.Y;
            }
        }
        private static Map map;



        internal static void Main(string[] args) {
            Console.CursorVisible = false;
            Console.BufferHeight = 50;

            int mainMenuPointer = 0;
            PrintMainMenu(mainMenuPointer);

            do {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Enter) {
                    Console.Clear();
                    StartGame();
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
        private static void StartGame() {
            map = new Map(mapPath);
            MapCursorPosition = default;

            do {
                Console.Clear();
                PrintMap(map);
                PrintGameMenu();
                HightlightCurrentMapTile();
                PrintTileInformation(MapCursorPosition);
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.DownArrow) {
                    ++MapCursorPositionY;
                }
                else
                if (input.Key == ConsoleKey.UpArrow) {
                    --MapCursorPositionY;
                }
                else
                if (input.Key == ConsoleKey.LeftArrow) {
                    --MapCursorPositionX;
                }
                else
                if (input.Key == ConsoleKey.RightArrow) {
                    ++MapCursorPositionX;
                }
            } while (true);
        }
        private static void PrintMap(Map map) {
            for (int r = 0; r < map.LengthY; r++) {
                for (int c = 0; c < map.LengthX; c++) {
                    WriteColored(map[c, r].ToString(), map[c, r].Color);
                }
                Console.WriteLine();
            }
        }
        private static void PrintGameMenu() {
            Console.WriteLine(new string('-', Console.BufferWidth - 1));
            Console.WriteLine(buttonsInstruction);
        }
        private static void HightlightCurrentMapTile() {
            char tile = map[MapCursorPosition.X, MapCursorPosition.Y].ToChar();
            Point currentCursorPosition = new Point(Console.CursorLeft, Console.CursorTop);
            Console.SetCursorPosition(MapCursorPosition.X, MapCursorPosition.Y);
            WriteColored(tile, hightlitedMapTileColor);
            Console.SetCursorPosition(currentCursorPosition.X, currentCursorPosition.Y);
        }
        private static void PrintTileInformation(Point tileCoord) {
            MapTile tile = map[tileCoord.X, tileCoord.Y];
            string message = tile.Name + $" ({mapCursorPositionX}; {mapCursorPositionY})";
            Console.WriteLine(message);
        }

    }
}