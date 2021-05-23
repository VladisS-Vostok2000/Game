using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public readonly struct MaptileInfo {
        public Unit Unit { get; }
        public Landtile Land { get; }
        public Point Location { get; }

        public bool ContainsUnit => Unit != null;
        public bool ReachableForSelectedUnit { get; }
        public bool AvailableForSelectedUnitMove { get; }
        public bool SelectedUnitWay { get; }



        public MaptileInfo(Landtile landtile,
                           Point location,
                           Unit unit,
                           bool reachableForSelectedUnit,
                           bool availableForSelectedUnitMove,
                           bool selectedUnitWay) {
            Location = location;
            Unit = unit;
            Land = landtile;
            ReachableForSelectedUnit = reachableForSelectedUnit;
            AvailableForSelectedUnitMove = availableForSelectedUnitMove;
            SelectedUnitWay = selectedUnitWay;
        }



        public ConsoleImage ToConsoleImage() => ContainsUnit ? Unit.ConsoleImage : Land.ConsoleImage;

    }

}
