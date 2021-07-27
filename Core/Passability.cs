using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public sealed class Passability : ICloneable {
        public string Name { get; set; }

        public const float PassabilityCoeff = 100; // %
        public const int MinValue = 0;
        public const int MaxValue = 100;

        public IDictionary<string, int> Passabilities { get; set; }



        public int this[string landtileName] => Passabilities[landtileName];



        public Passability(string name, IDictionary<string, int> tilesPassability) {
            Passabilities = tilesPassability;
            Name = name;
        }



        public static bool Valid(int passabilityValue) => passabilityValue >= MinValue && passabilityValue <= MinValue;


        public object Clone() => MemberwiseClone();

    }
}
