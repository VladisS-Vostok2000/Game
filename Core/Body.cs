using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public sealed class Body : Part {
        public Body(string name) : base(name) { }
        public Body(string name, string displayedName, int maxHP, int masse) : base(name, displayedName, maxHP, masse) { }

    }
}
