using Game;
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
    // REFACTORING: убрать Console? Тогда реализовывать консольные контролы отдельными классами.
    // Так-то по сути контролы с консолью не связаны.
    public interface IConsoleDrawable : IColoredCharsDrawable {
        Point Location { get; set; }
        int X { get; set; }
        int Y { get; set; }

    }
}
