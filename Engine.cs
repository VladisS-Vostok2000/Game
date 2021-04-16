using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Engine : Part {
        public int Power { get; set; }



        public Engine(string name) : base(name) { }

    }
}
