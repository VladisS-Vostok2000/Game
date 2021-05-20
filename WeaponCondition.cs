using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public class WeaponCondition : PartCondition {
        public Weapon Weapon => (Weapon)Part;


        public float CurrentCooldown { get; set; }



        public WeaponCondition(Weapon weapon) : base(weapon) {

        }

    }
}
