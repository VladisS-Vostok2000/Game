using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public sealed class Unit : IConsoleDrawable {
        private ConsoleImage consoleImage;
        public ConsoleImage ConsoleImage {
            get => SpecificColor ? consoleImage : new ConsoleImage(consoleImage.Char, Team.Color);
            set => consoleImage = value;
        }
        public ConsoleColor ConsoleColor {
            get => ConsoleImage.Color;
            set => consoleImage.Color = value;
        }
        public char ConsoleChar {
            get => ConsoleImage.Char;
            set => consoleImage.Char = value;
        }
        public bool SpecificColor { get; set; }
        public string DisplayedName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        // FEATURE: создать вычисление массы согласно броне и запчастей.
        public int Masse { get; set; } = 500;

        public Body Body { get; set; }
        public Chassis Chassis { get; set; }
        public Engine Engine { get; set; }


        // Map
        public Point Location {
            get => new Point(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public float TimeReserve { get; set; }
        public Point SecondLocation { get; set; }
        public IList<Point> Route { get; set; } = new List<Point>();
        public Team Team { get; set; }


        public float CalculateSpeedOnLandtile(string landtileName) => Engine.Power * Chassis.Passability[landtileName] * Engine.PowerCoeff / Passability.PassabilityCoeff / Masse;

    }
}
