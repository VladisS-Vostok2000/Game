using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.TextBox {
    public class TextBoxInvalidArgumentException : TextBoxException {
        public object ActualValue { get; }



        public TextBoxInvalidArgumentException(string message, object actualValue) : base(message) => ActualValue = actualValue;
        public TextBoxInvalidArgumentException(string message, Exception innerException) : base(message, innerException) { }

    }
}
