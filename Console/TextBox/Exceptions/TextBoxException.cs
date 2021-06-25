using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.TextBox {
    public class TextBoxException : Exception {
        public string Comment { get; }



        public TextBoxException(string comment) => Comment = comment;

    }
}
