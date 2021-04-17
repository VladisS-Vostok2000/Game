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
        public bool ClosestToSelectedUnit { get; }
        public bool AvailableForUnitMove => ClosestToSelectedUnit && ReachableForSelectedUnit;
        public bool SelectedUnitRoute { get; }



        public MapTileInfo(Landtile landtile,
                           Unit unit,
                           bool reachableForSelectedUnit,
                           bool closestToSelectedUnit,
                           bool selectedUnitRoute) {
            Unit = unit;
            Land = landtile;
            ReachableForSelectedUnit = reachableForSelectedUnit;
            ClosestToSelectedUnit = closestToSelectedUnit;
            SelectedUnitRoute = selectedUnitRoute;
        }



        public ConsoleImage ToConsoleImage() => ContainsUnit ? Unit.ConsoleImage : Land.ConsoleImage;

    }
}
