using System;

namespace Game {
    public interface IConsoleDrawable {
        ConsoleImage ConsoleImage { get; set; }
        ConsoleColor ConsoleColor { get; set; }
        char ConsoleChar { get; set; }

    }
}
