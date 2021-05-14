using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public readonly struct MaptileInfo {
        public Unit Unit { get; }
        public Landtile Land { get; }

        public bool ContainsUnit => Unit != null;
        public bool ReachableForSelectedUnit { get; }
        public bool AvailableForSelectedUnitMove { get; }



        public MaptileInfo(Landtile landtile,
                           Unit unit,
                           bool reachableForSelectedUnit,
                           bool availableForSelectedUnitMove) {
            Unit = unit;
            Land = landtile;
            ReachableForSelectedUnit = reachableForSelectedUnit;
            AvailableForSelectedUnitMove = availableForSelectedUnitMove;
        }



        public ConsoleImage ToConsoleImage() => ContainsUnit ? Unit.ConsoleImage : Land.ConsoleImage;

    }
}
