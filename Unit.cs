using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Unit : IConsoleDrawable {
        public ConsoleImage ConsoleImage { get; set; }
        public string Name { get; set; } = "Default";

        public Point Location { get; set; }

        public Body Body { get; private set; }
        public Chassis Chassis { get; private set; }



        public Unit() { }



        public void SetBody(string bodyName, UnitConfigurator configurator) => Body = configurator.GetBody(bodyName);
        public void SetChassis(string chassisName, UnitConfigurator configurator) => Chassis = configurator.GetChassis(chassisName);

    }
}
