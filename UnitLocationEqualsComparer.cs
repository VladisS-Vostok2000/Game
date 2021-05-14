using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class UnitLocationEqualsComparer : IEqualityComparer<Unit> {
        public bool Equals(Unit x, Unit y) => x.Location == y.Location;
        public int GetHashCode(Unit unit) => unit.Location.GetHashCode();

    }
}
