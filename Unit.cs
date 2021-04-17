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


        public Body Body { get; set; }
        public Chassis Chassis { get; set; }
        public Engine Engine { get; set; }
        // TODO: удалить лишнее поле.
        public Point Location { get; set; }


        // Map
        public float ReservedTime { get; set; } = 5;
        public IList<Point> UnitPath { get; set; }



        public float CalculateSpeed(string landtileName) => Engine.Power * Chassis.Passability[landtileName] * Engine.PowerCoeff / Passability.PassabilityCoeff / Masse;

    }
}
