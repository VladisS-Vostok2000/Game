using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Engine : Part {
        /// <summary>
        /// За сколько массы 1 мощность даст 1 скорость.
        /// </summary>
        public const float PowerCoeff = 100;
        public int Power { get; set; }



        public Engine(string name) : base(name) { }

    }
}
