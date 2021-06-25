using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.TextBox {
    public class TextBoxInvalidArgumentException : TextBoxException {
        public object Causer { get; }



        public TextBoxInvalidArgumentException(string comment, object causer) : base(comment) => Causer = causer;

    }

}
