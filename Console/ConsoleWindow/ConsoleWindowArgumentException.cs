using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.ConsoleWindow {
    public class ConsoleWindowArgumentException : ConsoleWindowException {
        public object Causer { get; }



        public ConsoleWindowArgumentException(string comment, object argument) : base(comment) {
            Causer = argument;
        }

    }
}
