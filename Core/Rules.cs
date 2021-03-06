using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core {
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
        public ICollection<NamedRoute> Routes { get; set; }
        // FEATURE: команды заполняются рандомно; можно задать функцию первой команды.
        public IList<Team> Teams { get; set; }



        // REFACTORING: возможно, следует сделать INamable, а потом задавать делегат, а то по сути тут повторяется код.
        public Body GetBody(string bodyName) => Bodies.First((Body body) => body.Name == bodyName);
        public Chassis GetChassis(string chassisName) => Chassis.First((Chassis chassis) => chassis.Name == chassisName);
        public Engine GetEngine(string engineName) => Engines.First((engine) => engine.Name == engineName);
        public Team GetTeam(string teamName) => Teams.First((team) => team.Name == teamName);
        public Weapon GetWeapon(string weaponName) => Weapons.First((weapon) => weapon.Name == weaponName);
        public Passability GetPassability(string passabilityName) => (Passability)Passabilities.First((passability) => passability.Name == passabilityName).Clone();
        internal Warhead GetWarhead(string warheadName) => (Warhead)Warheads.First((warhead) => warhead.Name == warheadName);
        internal Projectile GetProjectile(string projectileName) => (Projectile)Projectiles.First((projectile) => projectile.Name == projectileName);
        internal NamedRoute GetNamedRoute(string routeName) => Routes.First((namedRoute) => namedRoute.Name == routeName);

    }
}
