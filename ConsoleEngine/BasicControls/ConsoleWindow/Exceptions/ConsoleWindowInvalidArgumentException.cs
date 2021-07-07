using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsoleWindowInvalidArgumentException : ConsoleWindowException {
        public object ActualValue { get; }



        public ConsoleWindowInvalidArgumentException(string comment, object actualValue) : base (comment) {
            ActualValue = actualValue;
        }
        public ConsoleWindowInvalidArgumentException(string comment, Exception innerException) : base(comment, innerException) { }

    }
}
