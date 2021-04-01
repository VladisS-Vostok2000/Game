using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    internal sealed class MapTile {
        private char associatedChar;
        internal string Name { get; }
        internal ConsoleColor Color { get; }



        internal MapTile(char associatedChar) {
            switch (associatedChar) {
                case '"':
                    Name = "Земля";
                    Color = ConsoleColor.DarkRed;
                    break;
                case '~':
                    Name = "Вода";
                    Color = ConsoleColor.Blue;
                    break;
                case '^':
                    Name = "Скалы";
                    Color = ConsoleColor.White;
                    break;
                default:
                    throw new Exception();
            }
            this.associatedChar = associatedChar;
        }



        public override string ToString() => associatedChar.ToString();
        internal char ToChar() => associatedChar;

    }
}
