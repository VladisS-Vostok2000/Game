using System;

namespace Game {
    public sealed class Projectile : IConsoleDrawable {
        private ConsoleImage consoleImage;
        public ConsoleImage ConsoleImage { get => consoleImage; set => consoleImage = value; }
        public ConsoleColor Color { get => ConsoleImage.Color; set => consoleImage.Color = value; }
        public char ConsoleChar { get => ConsoleImage.Char; set => consoleImage.Char = value; }
        
        public string Name { get; set; }
        public Warhead Warhead { get; set; }



        public Projectile(string name) {
            Name = name;
        }

    }
}