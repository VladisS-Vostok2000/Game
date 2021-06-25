using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console {

    public interface IConsoleDrawable : IConsolePicture {
        Point Location { get; set; }
        int X { get; set; }
        int Y { get; set; }

    }
}
