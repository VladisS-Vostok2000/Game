using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleControlsEngine.Controls {
    public class ColoredCharPanelInvalidArgumentException : ColoredCharPanelException {
        public object ActualValue { get; }



        public ColoredCharPanelInvalidArgumentException(string comment, object actualValue) : base(comment) {
            ActualValue = actualValue;
        }
        public ColoredCharPanelInvalidArgumentException(string comment, Exception innerException) : base(comment, innerException) { }

    }
}
