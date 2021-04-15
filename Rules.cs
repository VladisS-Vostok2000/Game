using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undefinded;

namespace Game {
    public sealed class Rules {
        // TODO: рассмотреть возможность превратить в словари (классы?).
        public ICollection<Landtile> Landtiles { get; set; }
        public ICollection<Passability> Passabilities { get; set; }
        public ICollection<Body> Bodies { get; set; }
        public ICollection<Chassis> Chassis { get; set; }



        public Rules(ICollection<Landtile> landtiles, ICollection<Passability> passabilities, ICollection<Body> bodies, ICollection<Chassis> chassis) {
            Landtiles = landtiles;
            Passabilities = passabilities;
            Bodies = bodies;
            Chassis = chassis;
        }



        internal Body GetBody(string bodyName) => (Body)Bodies.First((Body _body) => _body.Name == bodyName).Clone();
        internal Chassis GetChassis(string chassisName) => (Chassis)Chassis.First((Chassis _chassis) => _chassis.Name == chassisName).Clone();

    }
}
