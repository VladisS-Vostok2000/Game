using System;
using System.Collections.Generic;
using System.Linq;

using Game.BasicTypesLibrary;

namespace Game.Core {
    public class WeaponCondition : PartCondition {
        public Weapon Weapon => (Weapon)Part;


        private float currentCooldown;
        public float CurrentCooldown {
            get => currentCooldown;
            set => currentCooldown = value.NotNegative();
        }



        public WeaponCondition(Weapon weapon) : base(weapon) {

        }

    }
}
