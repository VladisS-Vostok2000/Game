using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.ConsoleControlsEngine {
    public class ConsoleTextBoxException : Exception {
        public ConsoleTextBoxException(string message) : base(message) { }
        public ConsoleTextBoxException(string message, Exception innerException) : base(message, innerException) { }

    }
}
