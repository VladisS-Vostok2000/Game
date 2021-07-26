using System;
using System.Drawing;

namespace ConsoleEngine {
    /// <summary>
    /// Объект, реализующий этот интерфейс, имеет графическую интерпретацию символами.
    /// </summary>
    public interface IColoredCharsDrawable {
        Picture ConsolePicture { get; }
        Size Size { get; }

    }
}
