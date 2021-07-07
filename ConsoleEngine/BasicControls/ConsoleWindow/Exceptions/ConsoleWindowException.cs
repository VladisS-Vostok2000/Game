using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsoleWindowException : Exception {
        public ConsoleWindowException(string message) : base(message) { }
        public ConsoleWindowException(string message, Exception innerException) : base(message, innerException) { }

    }
}
