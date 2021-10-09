using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleControlsEngine {
    public class ConsoleTextBoxInvalidArgumentException : ConsoleTextBoxException {
        public object ActualValue { get; }



        public ConsoleTextBoxInvalidArgumentException(string message, object actualValue) : base(message) => ActualValue = actualValue;
        public ConsoleTextBoxInvalidArgumentException(string message, Exception innerException) : base(message, innerException) { }

    }
}
