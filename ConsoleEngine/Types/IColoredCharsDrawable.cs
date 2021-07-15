using System;

namespace ConsoleEngine {
    /// <summary>
    /// Объект, реализующий этот интерфейс, имеет графическую интерпретацию символами.
    /// </summary>
    public interface IColoredCharsDrawable {
        Picture ConsolePicture { get; }

    }
}
