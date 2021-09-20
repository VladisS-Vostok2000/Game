using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ColoredCharsEngine;
using Game.BasicTypesLibrary.ExtensionMethods;

namespace Game.Core {
    // REFACTORING: Подключить IDrawable, чтобы в GameMap использовать интерфейс, а не частный тип.
    // Ну или как-то там наоборот, не ебу сейчас.
    // ISSUE: сделать Unit отдельно от GameMap? А то как-то странно он прорисовывается.
    // Сначала в GameMap, потом в буфере. Пусть живёт сам по себе?
    // Тогда выделение тайла будет реализовано интересно точно.
    public sealed class Unit {
        public ColoredChar ColoredChar {
            get {
                ConsoleColor color = Flashed ? FlashColor : SpecificColor ? Color : Team.Color;
                return new ColoredChar(ConsoleChar, color);
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
        public ChassisCondition ChassisCondition { get; set; }
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
            return EngineCondition.Engine.Power * ChassisCondition.Chassis.Passability[landtileName] * PowerCoeff / Passability.PassabilityCoeff / Masse;
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
            bool valid = Location.CloseTo(newRoute[0]);
            if (!newRoute.Empty && !valid) { return false; }

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

            bool valid = lastWay.CloseTo(way);
            if (!valid) { throw new ArgumentException($"{nameof(way)} обязан стыковаться с последним тайлом пути."); }

            route.Add(way);
        }

        /// Дополнит марштрут <see cref="Unit"/>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddRoute(IEnumerable<Point> addingRoute) {
            foreach (var way in addingRoute) {
                AddWay(way);
            }
        }

        public void RemoveLastWay() {
            if (route.Empty) { throw new InvalidOperationException("Маршрут пуст."); }
            route.RemoveLast();
        }

        public void GetAttacked(Warhead warhead) {
            CurrentHP -= warhead.Damage;
            BodyCondition.CurrentHP -= warhead.Damage;
            ChassisCondition.CurrentHP -= warhead.Damage;
            FlashTimer += 2;
        }


        public override string ToString() {
            return DisplayedName;
        }

    }
}
