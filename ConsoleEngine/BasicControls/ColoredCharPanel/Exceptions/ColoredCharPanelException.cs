﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ColoredCharPanelException : Exception {
        public ColoredCharPanelException(string message) : base(message) { }
        public ColoredCharPanelException(string message, Exception innerException) : base(message, innerException) { }

    }
}
