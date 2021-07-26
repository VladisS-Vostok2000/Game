using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    /// <summary>
    /// Имеющий графическую интерпретацию объект.
    /// </summary>
    public abstract class Picture {
        public Size Size { get; protected set; }
        public int Width => Size.Width;
        public int Height => Size.Height;

    }
}
