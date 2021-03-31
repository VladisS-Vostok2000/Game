using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    internal sealed class MapTile {
        private char associatedChar;



        MapTile(char associatedChar) {
            this.associatedChar = associatedChar;
        }



        public override string ToString() => associatedChar.ToString();

    }
}
