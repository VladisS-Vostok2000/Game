using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public readonly struct MapTileInfo {
        public Unit Unit { get; }
        public Landtile Land { get; }

        public bool ContainsUnit => Unit != null;
        public bool ReachableForSelectedUnit { get; }
        public bool AvailableForSelectedUnitMove { get; }
        public bool SelectedUnitRoute { get; }



        public MapTileInfo(Landtile landtile,
                           Unit unit,
                           bool reachableForSelectedUnit,
                           bool availableForSelectedUnitMove,
                           bool selectedUnitRoute) {
            Unit = unit;
            Land = landtile;
            ReachableForSelectedUnit = reachableForSelectedUnit;
            AvailableForSelectedUnitMove = availableForSelectedUnitMove;
            SelectedUnitRoute = selectedUnitRoute;
        }



        public ConsoleImage ToConsoleImage() => ContainsUnit ? Unit.ConsoleImage : Land.ConsoleImage;

    }
}
