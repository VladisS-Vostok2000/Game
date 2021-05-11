using System;

namespace Game {
    public interface IConsoleDrawable {
        ConsoleImage ConsoleImage { get; set; }
        ConsoleColor Color { get; set; }
        char ConsoleChar { get; set; }

    }
}
