using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Weapon : Part {
        public Projectile Projectile { get; set; }
        public float Cooldown { get; set; }
        public float CurrentCooldown { get; set; }



        public Weapon(string name) : base(name) { }
        public Weapon(string name, string displayedName, int maxHP, int masse, Projectile projectile) : base(name, displayedName, maxHP, masse) {
            Projectile = projectile;
        }

    }
}
