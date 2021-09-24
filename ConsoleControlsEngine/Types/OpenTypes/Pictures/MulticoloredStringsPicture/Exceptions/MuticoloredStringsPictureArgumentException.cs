using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleDrawingEngine {
    class MuticoloredStringsPictureArgumentException : MulticoloredStringsPictureException {
        public object ActualValue { get; }



        public MuticoloredStringsPictureArgumentException(object actualValue, string message) : base(message) {
            ActualValue = actualValue;
        }

    }
}
