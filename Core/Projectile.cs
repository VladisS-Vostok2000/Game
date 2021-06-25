using System;

namespace Core {
    [Obsolete]
    public sealed class Projectile : IConsolePicture {
        private ConsoleImage consoleImage;
        public ConsoleImage ConsoleImage { get => consoleImage; set => consoleImage = value; }
        public ConsoleColor Color { get => ConsoleImage.Color; set => consoleImage.Color = value; }
        public char ConsoleChar { get => ConsoleImage.Char; set => consoleImage.Char = value; }
        
        public string Name { get; set; }



        public Projectile(string name) {
            Name = name;
        }

    }
}