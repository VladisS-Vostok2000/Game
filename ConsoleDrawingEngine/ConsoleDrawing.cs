using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ExtensionMethods;
using Game.ColoredCharsEngine;
using static System.Console;
using static Game.ExtensionMethods.BasicTypesExtensionsMethods;

namespace Game.ConsoleDrawingEngine {
    public static class ConsoleDrawing {
        public static int Width => BufferWidth - 1;
        public static int Height => BufferHeight;
        public static int LineFreeSpace => Width - CursorLeft;
        public static int FreeLines => BufferHeight - CursorTop - 1;



        /// <summary>
        /// Печатает элемент и переводит каретку вправо, но не до границы буфера.
        /// </summary>
        public static void Write(string str) {
            Console.Write(str.StringPart(LineFreeSpace));
        }
        /// <summary>
        /// Печатает элемент и переводит каретку вправо, но не до границы буфера.
        /// </summary>
        public static void Write(char chr) {
            if (LineFreeSpace > 0) {
                Console.Write(chr);
            }
        }
        /// <summary>
        /// Печатает элемент и переводит каретку вправо, но не до границы буфера.
        /// </summary>
        public static void WriteColored(MulticoloredStringBuilder multicoloredString) {
            foreach (var coloredString in multicoloredString) {
                WriteColored(coloredString);
            }
        }
        /// <summary>
        /// Печатает элемент и переводит каретку вправо, но не до границы буфера.
        /// </summary>
        public static void WriteColored(ColoredString coloredString) => WriteColored(coloredString.Text, coloredString.Color);
        /// <summary>
        /// Печатает элемент и переводит каретку вправо, но не до границы буфера.
        /// </summary>
        public static void WriteColored(string str, ConsoleColor color) {
            ForegroundColor = color;
            Write(str);
        }
        /// <summary>
        /// Печатает элемент и переводит каретку вправо, но не до границы буфера.
        /// </summary>
        public static void WriteColored(char chr, ConsoleColor color) {
            ForegroundColor = color;
            Write(chr);
        }
        /// <summary>
        /// Печатает элемент и переводит каретку вправо, но не до границы буфера.
        /// </summary>
        public static void WriteColored(ColoredChar charImage) => WriteColored(charImage.Char, charImage.Color);


        /// <summary>
        /// Вовзратит каретку вбок на заданное положение, и переведёт её вниз на строку; если есть место.
        /// </summary>
        public static void LineDown(int x) {
            if (CursorTop < Height) {
                CursorTop++;
                CursorLeft = x;
            }
        }

    }
}
