using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undefinded;

namespace Game {
    public sealed class Unit : IConsoleDrawable {
        private ConsoleImage consoleImage;
        public ConsoleImage ConsoleImage {
            get => SpecificColor ? consoleImage : new ConsoleImage(consoleImage.Char, Team.Color);
            set => consoleImage = value;
        }
        public bool SpecificColor { get; set; }
        public ConsoleColor Color {
            get => ConsoleImage.Color;
            set => consoleImage.Color = value;
        }
        public char ConsoleChar {
            get => ConsoleImage.Char;
            set => consoleImage.Char = value;
        }
        public string DisplayedName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        // FEATURE: создать вычисление массы согласно броне и запчастей.
        public int Masse { get; set; } = 500;

        public Body Body { get; set; }
        public Chassis Chassis { get; set; }
        public Engine Engine { get; set; }
        public Weapon Weapon { get; set; }


        // Map
        public Point Location {
            get => new Point(X, Y);
            private set {
                X = value.X;
                Y = value.Y;
            }
        }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Point NextLocation {
            get {
                if (route.Empty) { throw new InvalidOperationException(); }
                return route.First();
            }
        }
        public float TimeReserve { get; set; }
        private readonly Route route = new Route();
        public Team Team { get; set; }



        public Unit() { }
        public Unit(in int x, in int y) {
            X = x;
            Y = y;
        }
        public Unit(Point location) : this(location.X, location.Y) { }
        public Unit(in int x, in int y, Route route) : this (x, y) {
            bool valid = RouteStartCloselyToLocation(route.Top);
            if (!valid) { throw new InvalidOperationException(); }

            this.route = route;
        }
        public Unit(Point location, Route route) : this(location.X, location.Y, route) { }



        public float CalculateSpeedOnLandtile(string landtileName) => Engine.Power * Chassis.Passability[landtileName] * Engine.PowerCoeff / Passability.PassabilityCoeff / Masse;


        public IReadOnlyCollection<Point> GetRoute() => route.ToList();

        /// <summary>
        /// Перемещает Unit на одну следующую позицию Route.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Move() {
            Point nextLocation = route.Pop();
            Location = nextLocation;
        }

        /// <summary>
        /// Изменит маршрут Unit на корректно заданный.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetRoute(Route newRoute) {
            if (!newRoute.Empty) {
                bool valid = RouteStartCloselyToLocation(newRoute[0]);
                if (!valid) { throw new InvalidOperationException(); }
            }

            route.Overwrite(newRoute);
        }
        /// <exception cref="InvalidOperationException"></exception>
        public void AddWay(Point way) {
            if (route.Empty) {
                bool valid = RouteStartCloselyToLocation(way);
                if (!valid) { throw new InvalidOperationException(); }
            }

            route.Add(way);
        }
        /// <exception cref="InvalidOperationException"></exception>
        internal void AddRoute(IEnumerable<Point> appendedRoute) {
            foreach (var way in appendedRoute) {
                AddWay(way);
            }
        }
        private bool RouteStartCloselyToLocation(Point routeStart) => ExtensionsMethods.TilesClosely(Location, routeStart);

    }
}
