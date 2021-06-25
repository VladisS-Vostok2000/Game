using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public sealed class Chassis : Part {
        // FEATURE: добавить ограничение максимальной скорости на участке.
        public Passability Passability { get; set; }



        public Chassis(string name) :base(name) { }
        public Chassis(string name, string displayedName, int maxHP, int masse, Passability passability) : base(name, displayedName, maxHP, masse) {
            Passability = passability;
        }


    }
}
