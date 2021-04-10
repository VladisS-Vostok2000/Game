using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    // TODO: определить в структуру?
    public sealed class LandTile : IConsoleDrawable, ICloneable {
        public string Name { get; set; }
        public string DisplayedName { get; set; } = "Default";
        public ConsoleImage ConsoleImage { get; set; }



        public LandTile() { }
        public LandTile(string name, ConsoleImage consoleImage) {
            DisplayedName = name;
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
