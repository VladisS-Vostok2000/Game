using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game {
    /// <summary>
    /// Инкапсулирует списки частей Unit.
    /// </summary>
    public sealed class UnitConfigurator {
        public ICollection<Body> Bodies { get; private set; }
        public ICollection<Chassis> Chassis { get; private set; }



        public UnitConfigurator(ICollection<Body> bodies, ICollection<Chassis> chassis) {
            Bodies = bodies;
            Chassis = chassis;
        }



        public void ConfigurateUnit(Unit unit, string body, string chassis) {
            unit.SetBody(body, this);
            unit.SetChassis(chassis, this);
        }
        public Body GetBody(string bodyName) {
            foreach (var body in Bodies) {
                if (body.Name == bodyName) {
                    return (Body)body.Clone();
                }
            }
            throw new Exception();
        }
        public Chassis GetChassis(string chassisName) {
            foreach (var chassis in Chassis) {
                if (chassis.Name == chassisName) {
                    return (Chassis)chassis.Clone();
                }
            }
            throw new Exception();
        }

    }
}
