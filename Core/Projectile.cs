using ConsoleEngine;
using System;

namespace Core {
    [Obsolete]
    public sealed class Projectile {
        private ColoredChar charPicture;
        public ColoredChar CharPicture { get => charPicture; set => charPicture = value; }
        public ConsoleColor Color { get => charPicture.Color; set => charPicture.Color = value; }
        public char ConsoleChar { get => charPicture.Char; set => charPicture.Char = value; }
        


        public string Name { get; set; }



        public Projectile(string name) {
            Name = name;
        }

    }
}