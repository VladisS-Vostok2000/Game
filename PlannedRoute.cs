using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undefinded;

namespace Game {
    public sealed class PlannedRoute : Route {
        public string Name { get; private set; }



        public PlannedRoute(string routeName) => Name = routeName;

    }
}
