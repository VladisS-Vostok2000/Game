using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public class RouteInvalidOperationException : RouteException {
        public RouteInvalidOperationException(string message) : base(message) { }

    }
}
