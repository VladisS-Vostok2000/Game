using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

namespace Core {
    public sealed class Unit : IConsolePicture {
        public ConsoleImage ConsoleImage {
            get {
                ConsoleColor color = Flashed ? FlashColor : SpecificColor ? Color : Team.Color;
                return new ConsoleImage(ConsoleChar, color);
            }
            set {
                color = value.Color;
                consoleChar = value.Char;
            }
        }
        public bool SpecificColor { get; set; }
        private ConsoleColor color;
        public ConsoleColor Color {
            get => color;
            set => color = value;
        }
        private char consoleChar;
        public char ConsoleChar {
            get => consoleChar;
            set => consoleChar = value;
        }

        public bool Flashed => FlashTimer > 0;
        private float flashTimer;
        public float FlashTimer {
            get => flashTimer;
            set => flashTimer = value.NotNegative();
        }
        public const ConsoleColor FlashColor = ConsoleColor.Cyan;

        public string DisplayedName { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        // FEATURE: создать вычисление массы согласно броне и запчастей.
        public int Masse { get; set; } = 500;

        public BodyCondition BodyCondition { get; set; }
        public ChassisCondition ChassisCondition { get;set; }
        public EngineCondition EngineCondition { get; set; }
        public WeaponCondition WeaponCondition { get; set; }


        /// <summary>
        /// За сколько массы 1 мощность даст 1 скорость.
        /// </summary>
        public const float PowerCoeff = 100;


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
        // REFACTORING: теперь кажется, что логику времени лучше инкапсулировать
        // внутри unit. Route инкапсулирован же.
        public float TimeReserve { get; set; }
        private readonly Route route = new Route();
        public Team Team { get; set; }



        public Unit(Point location, BodyCondition body, ChassisCondition chassis, EngineCondition engine, WeaponCondition weapon) : this(location.X, location.Y, body, chassis, engine, weapon) { 
        
        }
        public Unit(in int x, in int y, BodyCondition body, ChassisCondition chassis, EngineCondition engine, WeaponCondition weapon) {
            X = x;
            Y = y;
            BodyCondition = body;
            ChassisCondition = chassis;
            EngineCondition = engine;
            WeaponCondition = weapon;
        }



        public float CalculateSpeedOnLandtile(string landtileName) {
            return  EngineCondition.Engine.Power * ChassisCondition.Chassis.Passability[landtileName] * PowerCoeff / Passability.PassabilityCoeff / Masse;
        }

        public IReadOnlyCollection<Point> GetRoute() => route.ToList();

        public int RouteLength => route.Count();

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
        public bool TrySetRoute(Route newRoute) {
            if (!newRoute.Empty && !BasicTypesExtensionsMethods.TilesClosely(Location, newRoute[0])) { return false; }

            route.Overwrite(newRoute);
            return true;
        }

        /// Дополнит маршрут <see cref="Unit"/>.
        /// <exception cref="InvalidOperationException"></exception>
        public void AddWay(Point way) {
            Point lastWay;
            if (route.Empty) {
                lastWay = Location;
            }
            else {
                lastWay = route.Last();
            }

            if (!BasicTypesExtensionsMethods.TilesClosely(lastWay, way)) { throw new ArgumentException($"{nameof(way)} обязан стыковаться с последним тайлом пути"); }
            
            route.Add(way);
        }

        /// Дополнит марштрут <see cref="Unit"/>
        /// <exception cref="InvalidOperationException"></exception>
        public void AppendRoute(IEnumerable<Point> appendedRoute) {
            foreach (var way in appendedRoute) {
                AddWay(way);
            }
        }

        public void RemoveLastWay() {
            if (route.Empty) { throw new InvalidOperationException("Маршрут пуст."); }
            route.RemoveLast();
        }

        internal void GetAttacked(Warhead warhead) {
            CurrentHP -= warhead.Damage;
            BodyCondition.CurrentHP -= warhead.Damage;
            ChassisCondition.CurrentHP -= warhead.Damage;
            FlashTimer += 2;
        }

    }
}
