using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.ConsoleWindow {
    public class ConsoleWindowInvalidArgumentException : ConsoleWindowException {
        public object Causer { get; }
        public Exception InnerException { get; }



        public ConsoleWindowInvalidArgumentException(string comment, object causer) : base (comment) {
            Causer = causer;
        }
        public ConsoleWindowInvalidArgumentException(string comment, Exception innerException) : base(comment) {
            InnerException = innerException;
        }

    }
}
