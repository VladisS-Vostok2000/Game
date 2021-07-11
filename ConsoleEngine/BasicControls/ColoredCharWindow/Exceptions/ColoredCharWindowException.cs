using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ColoredCharWindowException : Exception {
        public ColoredCharWindowException(string message) : base(message) { }
        public ColoredCharWindowException(string message, Exception innerException) : base(message, innerException) { }

    }
}
