//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Game.ConsoleDrawingEngine;

//namespace Game.ConsoleDrawingEngine {
//    [Obsolete]
//    internal sealed class ConsoleListColoredString {
//        private bool @checked;
//        public bool Checked {
//            get => @checked;
//            set {
//                @checked = value;
//                ColoredString.Color = (@checked) ? CheckedColor : UncheckedColor;
//            }
//        }
//        public ColoredString ColoredString { get; set; }
//        public ConsoleColor CheckedColor { get; set; } = ConsoleColor.Red;
//        public ConsoleColor UncheckedColor { get; set; } = ConsoleColor.White;



//        public ConsoleListColoredString(string text, ConsoleColor color = ConsoleColor.White) {
//            ColoredString = new ColoredString(text, color);
//            Checked = false;
//        }

//    }
//}
