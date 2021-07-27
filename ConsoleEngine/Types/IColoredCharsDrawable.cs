using System;
using System.Drawing;

namespace Game.ConsoleEngine {
    /// <summary>
    /// Объект, реализующий этот интерфейс, имеет графическую интерпретацию символами.
    /// </summary>
    public interface IColoredCharsDrawable {
        Picture ConsolePicture { get; }
        Size Size { get; }

    }
}
