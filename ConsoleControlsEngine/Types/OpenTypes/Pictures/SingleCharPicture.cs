using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ConsoleDrawingEngine.Pictures {
    // REFACTORING: переименовать в PlanePicture. SingleChar будет 1х1.
    public class SingleCharPicture : Picture {
        public char Char { get; }
        public SingleCharPicture(Size size, char character) : base(size) {
            Char = character;
        }

    }
}
