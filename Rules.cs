using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undefinded;

namespace Game {
    public sealed class Rules {
        // REFACTORING: рассмотреть возможность превратить в словари (классы?).
        public ICollection<Landtile> Landtiles { get; set; }
        public ICollection<Passability> Passabilities { get; set; }
        public ICollection<Body> Bodies { get; set; }
        public ICollection<Chassis> Chassis { get; set; }
        public ICollection<Engine> Engines { get; set; }



        public Rules(ICollection<Landtile> landtiles, ICollection<Passability> passabilities, ICollection<Body> bodies, ICollection<Chassis> chassis, ICollection<Engine> engines) {
            Landtiles = landtiles;
            Passabilities = passabilities;
            Bodies = bodies;
            Chassis = chassis;
            Engines = engines;
        }



        internal Body GetBody(string bodyName) => (Body)Bodies.First((Body _body) => _body.Name == bodyName).Clone();
        internal Chassis GetChassis(string chassisName) => (Chassis)Chassis.First((Chassis _chassis) => _chassis.Name == chassisName).Clone();
        internal Engine GetEngine(string engineName) => (Engine)Engines.First((Engine _engine) => _engine.Name == engineName).Clone();

    }
}
