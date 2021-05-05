using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Weapon {
        public string Name { get; set; }
        public string DisplayedName { get; set; }
        public Projectile Projectile { get; set; }
        public float Cooldown { get; set; }
        public float CurrentCooldown { get; set; }



        public Weapon(string name) => Name = name;

    }
}
