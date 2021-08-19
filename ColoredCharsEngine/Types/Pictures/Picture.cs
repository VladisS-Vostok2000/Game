using Game.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ColoredCharsEngine {
    /// <summary>
    /// Имеющий графическую интерпретацию объект.
    /// </summary>
    public abstract class Picture {
        // ISSUE: Size должен быть private set, но нет возможности проверять null входных
        // данных, не имеющих ISize.
        public Size Size { get; private set; }
        public int Width => Size.Width;
        public int Height => Size.Height;



        public Picture(Size size) {
            if (size.IsEmptyOrFlat()) {
                throw new ArgumentException("Изображение должно иметь адекватный размер.", nameof(size));
            }

            Size = size;
        }

    }
}
