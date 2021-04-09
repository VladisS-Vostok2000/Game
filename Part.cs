using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public abstract class Part : ICloneable {
        public string Name { get; set; }
        public string DisplayedName { get; set; }
        public int MaxHP { get; set; } = 1;
        public int CurrentHP { get; set; } = 1;
        public int Masse { get; set; } = 1;



        public object Clone() => this.MemberwiseClone();

    }
}
