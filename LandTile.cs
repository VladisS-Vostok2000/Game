using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class LandTile : IConsoleDrawable, ICloneable {
        public string Name { get; set; } = "Default";
        public ConsoleImage ConsoleImage { get; set; }



        public LandTile() { }
        public LandTile(string name, ConsoleImage consoleImage) {
            Name = name;
            ConsoleImage = consoleImage;
        }



        public static LandTile Parse(char chr, ICollection<LandTile> landTiles) {
            foreach (var landTile in landTiles) {
                if (landTile.ConsoleImage.Char == chr) {
                    return (LandTile)landTile.Clone();
                }
            }
            throw new Exception();
        }

        public object Clone() => MemberwiseClone();

    }
}
