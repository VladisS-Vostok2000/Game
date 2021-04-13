using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Undefinded;

namespace Game {
    /// <summary>
    /// Инкапсулирует списки частей Unit.
    /// </summary>
    public sealed class UnitConfigurator : ICloneable {
        private ICollection<Body> Bodies { get; set; }
        private ICollection<Chassis> Chassis { get; set; }



        public UnitConfigurator(ICollection<Body> bodies, ICollection<Chassis> chassis) {
            Bodies = bodies;
            Chassis = chassis;
        }



        public object Clone() => new UnitConfigurator(Bodies.ToList(), Chassis.ToList());

    }
}
