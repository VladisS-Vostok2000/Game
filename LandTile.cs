using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class LandTile : IConsoleDrawable {
        public enum LandTileTypes {
            None,
            Land,
            Water,
            Rock,
        }

        public LandTileTypes Type { get; }
        public string Name { get; }

        public ConsoleImage ConsoleImage { get; set; }



        public LandTile(LandTileTypes tile) {
            switch (tile) {
                case LandTileTypes.Land:
                Name = "Земля";
                ConsoleImage = new ConsoleImage('"', ConsoleColor.DarkRed);
                break;
                case LandTileTypes.Water:
                Name = "Вода";
                ConsoleImage = new ConsoleImage('~', ConsoleColor.Blue);
                break;
                case LandTileTypes.Rock:
                Name = "Скалы";
                ConsoleImage = new ConsoleImage('^', ConsoleColor.Gray);
                break;
                default:
                throw new Exception();
            }
            Type = tile;
        }



        public static LandTileTypes CharToLandTileType(char chr) {
            switch (chr) {
                case '"':
                return LandTileTypes.Land;
                case '~':
                return LandTileTypes.Water;
                case '^':
                return LandTileTypes.Rock;
                default:
                return LandTileTypes.None;
            }
        }

    }
}
