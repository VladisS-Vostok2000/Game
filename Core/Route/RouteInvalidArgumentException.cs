using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public class RouteInvalidArgumentException : RouteException {
        public object ActualValue { get; }
        public RouteInvalidArgumentException(string message, object actualValue) : base(message) => ActualValue = actualValue;

    }
}
