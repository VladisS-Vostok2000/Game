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
        public ICollection<Warhead> Warheads { get; set; }
        public ICollection<Projectile> Projectiles { get; set; }
        public ICollection<Weapon> Weapons { get; set; }
        public ICollection<PlannedRoute> Routes { get; set; }
        public IList<Team> Teams { get; set; }



        // REFACTORING: возможно, следует сделать INamable, а потом задавать делегат, а то по сути тут повторяется код.
        public Body GetBody(string bodyName) => (Body)Bodies.First((Body body) => body.Name == bodyName).Clone();
        public Chassis GetChassis(string chassisName) => (Chassis)Chassis.First((Chassis chassis) => chassis.Name == chassisName).Clone();
        public Engine GetEngine(string engineName) => (Engine)Engines.First((engine) => engine.Name == engineName).Clone();
        public Team GetTeam(string teamName) => Teams.First((team) => team.Name == teamName);
        public Weapon GetWeapon(string weaponName) => (Weapon)Weapons.First((weapon) => weapon.Name == weaponName).Clone();
        public Passability GetPassability(string passabilityName) => (Passability)Passabilities.First((passability) => passability.Name == passabilityName).Clone();
        internal Warhead GetWarhead(string warheadName) => (Warhead)Warheads.First((warhead) => warhead.Name == warheadName).Clone();
        internal Projectile GetProjectile(string projectileName) => (Projectile)Projectiles.First((projectile) => projectile.Name == projectileName).Clone();
        internal PlannedRoute GetRoute(string routeName) => Routes.First((plannedRoute) => plannedRoute.Name == routeName);

    }
}
