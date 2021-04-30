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
        public IList<Team> Teams { get; set; }



        public Rules(ICollection<Landtile> landtiles, ICollection<Passability> passabilities, ICollection<Body> bodies, ICollection<Chassis> chassis, ICollection<Engine> engines, IList<Team> teams) {
            Landtiles = landtiles;
            Passabilities = passabilities;
            Bodies = bodies;
            Chassis = chassis;
            Engines = engines;
            Teams = teams;
        }



        internal Body GetBody(string bodyName) => (Body)Bodies.First((Body body) => body.Name == bodyName).Clone();
        internal Chassis GetChassis(string chassisName) => (Chassis)Chassis.First((Chassis chassis) => chassis.Name == chassisName).Clone();
        internal Engine GetEngine(string engineName) => (Engine)Engines.First((engine) => engine.Name == engineName).Clone();
        internal Team GetTeam(string teamName) => Teams.First((team) => team.Name == teamName);

    }
}
