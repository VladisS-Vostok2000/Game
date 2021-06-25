using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using static System.Console;

namespace Console {
    public static class ConsoleDrawing {
        public static void WriteColored(string str, ConsoleColor color) {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = currentColor;
        }
        public static void WriteLineColored(string str, ConsoleColor color) {
            WriteColored(str, color);
            Console.WriteLine();
        }
        public static void WriteColored(char chr, ConsoleColor color) => WriteColored(chr.ToString(), color);
        public static void WriteLineColored(char chr, ConsoleColor color) => WriteColored(chr.ToString(), color);
        public static void WriteColored(ConsoleImage charImage) => WriteColored(charImage.Char, charImage.Color);
        public static void WriteLineColored(ConsoleImage charImage) => WriteLineColored(charImage.Char, charImage.Color);
        public static void DrawSeparator() {
            int separatorsCount = Console.BufferWidth - 1 - Console.CursorLeft;
            Console.WriteLine(new string('-', separatorsCount));
        }
        

    }
}
