using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsolePictureException : Exception {



        public ConsolePictureException(string message) : base (message) { }
        public ConsolePictureException(string message, Exception innerException) : base (message, innerException) { }

    }
}
