using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public sealed class UnitLocationEqualsComparer : IEqualityComparer<Unit> {
        public bool Equals(Unit firstUnit, Unit secondUnit) => firstUnit.Location == secondUnit.Location;
        public int GetHashCode(Unit unit) => unit.Location.GetHashCode();

    }
}
