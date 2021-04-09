﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class MapTileInfo {
        public Unit Unit;
        public LandTile Land;

        public bool ContainsUnit => Unit != null;



        public MapTileInfo(LandTile landTile, Unit unit) {
            Unit = unit;
            Land = landTile;
        }

    }
}