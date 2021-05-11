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
        // REFACTORING: Существует ли эта сущность только как ключ словаря? Удалить её тогда?
        public string Name { get; private set; }



        public PlannedRoute(string routeName) => Name = routeName;

    }
}
