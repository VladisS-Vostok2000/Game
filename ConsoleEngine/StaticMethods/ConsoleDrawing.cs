using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using static System.Console;

namespace ConsoleEngine {
    public static class ConsoleDrawing {
        public static int Width => BufferWidth - 1;



        public static void WriteColored(string str, ConsoleColor color) {
            ConsoleColor currentColor = ForegroundColor;
            ForegroundColor = color;
            Write(str);
            ForegroundColor = currentColor;
        }
        public static void WriteLineColored(string str, ConsoleColor color) {
            WriteColored(str, color);
            WriteLine();
        }
        public static void WriteColored(char chr, ConsoleColor color) => WriteColored(chr.ToString(), color);
        public static void WriteLineColored(char chr, ConsoleColor color) => WriteColored(chr.ToString(), color);
        public static void WriteColored(ColoredChar charImage) => WriteColored(charImage.Char, charImage.Color);
        public static void WriteLineColored(ColoredChar charImage) => WriteLineColored(charImage.Char, charImage.Color);
        public static void Draw(IConsoleControl control) {
            if (control.ConsolePicture is ColoredCharsPicture ccp) {
                DrawColoredCharPicture(control);
            }
            else
                if (control.ConsolePicture is MulticoloredStringsPicture msp) {
                DrawColoredStringPicture(control);
            }
            else {
                throw new Exception();
            }
        }
        
        private static void DrawColoredCharPicture(IConsoleControl control) {
            if (IntersectsWithBuffer(control.Location, control.Size)) {

            }
            // TODO: intersectsWith
            //x = Width <= x ? x : Width;
            //y = BufferHeight <= y ? y : BufferHeight;
            //for (int r = 0; r < consolePicture.Height; r++) {
            //    CursorTop = y + r;
            //    for (int c = 0; c < consolePicture.Width; c++) {

            //    }
            //}
        }
        private static void DrawColoredStringPicture(IConsoleControl control) {

        }


        private static bool IntersectsWithBuffer(Point location, Size size) {
            // TODO:
            return true;
        }

    }
}
