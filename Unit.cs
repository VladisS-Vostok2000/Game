using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Unit : IConsoleDrawable {
        public ConsoleImage ConsoleImage { get; set; }
        public string DisplayedName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        // TODO: создать вычисление массы согласно броне и запчастей.
        public int Masse { get; set; } = 500;

        public Point Location { get; set; }

        public Body Body { get; set; }
        public Chassis Chassis { get; set; }
        public Engine Engine { get; set; }



        public float CalculateSpeed(string landtileName) => Engine.Power * Chassis.Passability[landtileName] * Engine.PowerCoeff / Passability.PassabilityCoeff / Masse;

    }
}
