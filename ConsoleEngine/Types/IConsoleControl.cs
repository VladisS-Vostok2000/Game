using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    /// <summary>
    /// Объект, реализующий данный интерфейс, имеет графическую интерпритацию
    /// и чёткое местоположение в окне консоли.
    /// </summary>
    public interface IConsoleControl : IConsoleDrawable {
        Point Location { get; set; }
        int X { get; set; }
        int Y { get; set; }

    }
}
