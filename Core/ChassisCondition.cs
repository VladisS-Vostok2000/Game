using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public sealed class ChassisCondition : PartCondition {
        public Chassis Chassis => (Chassis)Part;



        public ChassisCondition(Chassis chassis) : base(chassis) {
        
        }

    }
}
