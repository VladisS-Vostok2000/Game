using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.ConsoleWindow {
    public class ConsoleWindowException : Exception {
        public string Comment { get; }



        public ConsoleWindowException(string comment) => Comment = comment;

    }
}
