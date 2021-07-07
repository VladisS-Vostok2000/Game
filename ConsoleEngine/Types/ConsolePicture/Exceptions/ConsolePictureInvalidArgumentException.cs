using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    class ConsolePictureInvalidArgumentException : ConsolePictureException {
        public object ActualValue { get; }



        public ConsolePictureInvalidArgumentException(string comment, object actualValue) : base(comment) {
            ActualValue = actualValue;
        }
        public ConsolePictureInvalidArgumentException(string comment, Exception innerException) : base(comment, innerException) { }

    }
}
