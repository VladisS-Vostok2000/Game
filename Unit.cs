using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Unit : IConsoleDrawable {
        public ConsoleImage ConsoleImage { get; set; }
        public ConsoleColor ConsoleColor { get => ConsoleImage.Color; set => new ConsoleImage(ConsoleChar, value); }
        public char ConsoleChar { get => ConsoleImage.Char; set => new ConsoleImage(value, ConsoleColor); }
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
        public float TimeReserve { get; set; } = 5;
        // TODO: это будет происходить во время смены хода.
        public IList<Point> Route { get; set; } = new List<Point>();



        public float CalculateSpeed(string landtileName) => Engine.Power * Chassis.Passability[landtileName] * Engine.PowerCoeff / Passability.PassabilityCoeff / Masse;

    }
}
