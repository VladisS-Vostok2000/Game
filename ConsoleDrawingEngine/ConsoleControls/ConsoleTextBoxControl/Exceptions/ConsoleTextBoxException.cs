using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleDrawingEngine.ConsoleControls {
    public class ConsoleTextBoxException : Exception {
        public ConsoleTextBoxException(string message) : base(message) { }
        public ConsoleTextBoxException(string message, Exception innerException) : base(message, innerException) { }

    }
}
