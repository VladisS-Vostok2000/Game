using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Weapon : Part {
        public Warhead Warhead { get; set; }
        [Obsolete]
        public Projectile Projectile { get; set; }
        public float Cooldown { get; set; }



        public Weapon(string name) : base(name) { }

    }
}
