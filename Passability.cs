using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Passability : ICloneable {
        public string Name { get; set; } = "Default";

        private IDictionary<string, int> passabilities;
        public const int MinValue = 0;
        public const int MaxValue = 100;



        public int this[LandTile landTile] => passabilities[landTile.Name];
        public int this[string landTileName] => passabilities[landTileName];



        public Passability(ICollection<LandTile> landTiles) {
            passabilities = new Dictionary<string, int>();
            foreach (var landTile in landTiles) {
                passabilities.Add(landTile.Name, MaxValue);
            }
        }
        public Passability(IDictionary<LandTile, int> passabilityPairs) {
            passabilities = new Dictionary<string, int>();
            foreach (var passabilityPair in passabilityPairs) {
                string landTile = passabilityPair.Key.Name;
                int landTilePassability = passabilityPair.Value;
                passabilities.Add(landTile, landTilePassability);
            }
        }


        public bool Correct(int passabilityValue) => passabilityValue >= MinValue && passabilityValue <= MinValue;
        public object Clone() => MemberwiseClone();

    }
}
