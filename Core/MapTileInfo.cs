using Game.ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public readonly struct MaptileInfo {
        public Unit Unit { get; }
        public Landtile Land { get; }
        public Point Location { get; }
        public ColoredChar ColoredChar { get; }

        public bool ContainsUnit => Unit != null;
        // ISSUE: стоит ли здесь бросать эксепшены?
        public bool ReachableForSelectedUnit { get; }
        public bool AvailableForSelectedUnitMove { get; }
        public bool SelectedUnitWay { get; }


        // REFACTORING: разделить конструктор на два, в котором
        // значения полей несуществующего юнита синициализируются
        // на false автоматически?
        public MaptileInfo(Landtile landtile,
                           Point location,
                           Unit unit,
                           ColoredChar coloredChar,
                           bool reachableForSelectedUnit,
                           bool availableForSelectedUnitMove,
                           bool selectedUnitWay) {
            Location = location;
            Unit = unit;
            Land = landtile;
            ColoredChar = coloredChar;
            ReachableForSelectedUnit = reachableForSelectedUnit;
            AvailableForSelectedUnitMove = availableForSelectedUnitMove;
            SelectedUnitWay = selectedUnitWay;
        }

    }

}
