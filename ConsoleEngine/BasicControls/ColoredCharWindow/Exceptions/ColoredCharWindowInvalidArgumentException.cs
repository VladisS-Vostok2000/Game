using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ColoredCharWindowInvalidArgumentException : ColoredCharWindowException {
        public object ActualValue { get; }



        public ColoredCharWindowInvalidArgumentException(string comment, object actualValue) : base (comment) {
            ActualValue = actualValue;
        }
        public ColoredCharWindowInvalidArgumentException(string comment, Exception innerException) : base(comment, innerException) { }

    }
}
