using System;
namespace Game.Core {
    public sealed class Warhead {
        public string Name { get; private set; }
        public int Damage { get; set; }



        public Warhead(string name) {
            Name = name;
        }

    }
}