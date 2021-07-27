using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public class RouteException : Exception {
        public RouteException(string message) : base(message) { }

    }
}
