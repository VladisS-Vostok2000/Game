using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

namespace Core {
    public sealed class NamedRoute {
        public string Name { get; private set; }
        public Route Route { get; set; }



        public NamedRoute(string routeName, Route route) {
            Name = routeName;
            Route = route;
        }

    }
}
