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
        private static string buttonsInstruction = @"[→] [←] [↑] [↓]";

        private static Map map;
        private static Rules rules;


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
        private static void WriteColored(ConsoleImage charImage) => WriteColored(charImage.Char, charImage.Color);
        private static void WriteLineColored(ConsoleImage charImage) => WriteLineColored(charImage.Char, charImage.Color);


        private static void StartGame() {
            rules = new Rules(rulesPath);
            map = new Map(mapPath);

            do {
                Console.Clear();
                PrintMapScreen();
                PrintGameMenu();
                PrintSelectedTileInformation();
                ConsoleKeyInfo input = Console.ReadKey(true);
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
            } while (true);
        }
        private static void PrintMapScreen() {
            ConsoleImage[,] images = map.ToConsoleImages();
            for (int r = 0; r < map.LengthY; r++) {
                for (int c = 0; c < map.LengthX; c++) {
                    WriteColored(images[c, r]);
                }
                Console.WriteLine();
            }
        }
        private static void PrintGameMenu() {
            Console.WriteLine(new string('-', Console.BufferWidth - 1));
            Console.WriteLine(buttonsInstruction);
        }
        private static void PrintSelectedTileInformation() {
            ConsoleImage selectedTileImage = map.GetConsoleImage(map.SelectedTileLocation);
            Console.Write("[");
            WriteColored(selectedTileImage);
            Console.Write("] ");

            MapTileInfo selectedTileInfo = map.SelectedTile;
            string name = selectedTileInfo.Unit?.DisplayedName ?? selectedTileInfo.Land.DisplayedName;
            Console.WriteLine(name + $"({map.SelectedTileX}; {map.SelectedTileY})");
        }

    }
}