using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ExtensionMethods;

namespace Game.Core {
    public class Route : IEnumerable<Point> {
        private IList<Point> route = new List<Point>();
        public bool Empty => route.Count == 0;
        public Point Top => Empty ? throw new InvalidOperationException() : route[0];



        public Route() { }
        ///
        /// <exception cref="ArgumentException"></exception>
        public Route(IList<Point> sourse) {
            try {
                AddRange(sourse);
            }
            catch (RouteInvalidArgumentException riae) {
                throw riae;
            }
        }



        public Point this[int index] => Empty ? throw new InvalidOperationException("Маршрут пуст.") : route[index];


        /// 
        /// <exception cref="RouteInvalidArgumentException"></exception>
        public void Add(Point way) {
            if (Empty) {
                route.Add(way);
                return;
            }

            Point unitTempLocation = route.Last();
            bool valid = unitTempLocation.CloseTo(way);
            if (!valid) { throw new RouteInvalidArgumentException("Путь находится не в ближайшей конечной точке маршрута.", way); }

            route.Add(way);
        }
        /// 
        /// <exception cref="RouteInvalidArgumentException"></exception>
        public void AddRange(IList<Point> points) {
            if (points.Empty()) { return; }

            if (!Empty && !route.Last().CloseTo(points[0])) {
                throw new RouteInvalidArgumentException("Маршруты не стыкуются.", points);
            }

            if (!PointsContinual(points)) {
                throw new RouteInvalidArgumentException("Маршрут не последователен.", points);
            }

            route.AddRange(points);
        }
        /// <summary>
        /// Возвращает, удаляя из списка, следующую точку маршрута.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public Point Pop() {
            if (Empty) {
                throw new InvalidOperationException("Маршрут пуст.");
            }

            var out_value = route[0];
            route.RemoveAt(0);
            return out_value;
        }
        /// <summary>
        /// Копирует точки маршрута с заданного <see cref="Route"/>, удаляя прежние.
        /// </summary>
        public void Overwrite(Route newRoute) {
            route.Clear();
            route.AddRange(newRoute.route);
        }
        public void RemoveLast() {
            if (Empty) { throw new InvalidOperationException(); }
            route.RemoveLastItem();
        }


        // ISSUE: перенести в другое место?
        /// <summary>
        /// <see langword="true"/>, если все точки списка стоят вплотную друг к другу в одной из четырёх сторон.
        /// </summary>
        public static bool PointsContinual(IList<Point> points) {
            if (points.Empty()) { return true; }

            for (int i = 0; i < points.Count - 1; i++) {
                bool valid = points[i].CloseTo(points[i + 1]);
                if (!valid) {
                    return false;
                }
            }

            return true;
        }


        public IEnumerator<Point> GetEnumerator() => route.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
