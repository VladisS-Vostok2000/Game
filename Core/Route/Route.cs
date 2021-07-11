using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using static Core.MapMethods;

namespace Core {
    public class Route : IEnumerable<Point> {
        private IList<Point> route = new List<Point>();
        public bool Empty => route.Count == 0;
        public Point Top => Empty ? throw new InvalidOperationException() : route[0];



        public Route() { }
        ///
        /// <exception cref="ArgumentException"></exception>
        public Route(IList<Point> sourse) { AddRange(sourse); }



        public Point this[int index] => route[index];


        /// 
        /// <exception cref="RouteInvalidArgumentException"></exception>
        public void Add(Point way) {
            if (Empty) {
                route.Add(way);
                return;
            }

            Point unitTempLocation = route.Last();
            bool valid = TilesClosely(unitTempLocation, way);
            if (!valid) { throw new RouteInvalidArgumentException("Путь находится не в ближайшей конечной точке маршрута.", way); }

            route.Add(way);
        }
        /// 
        /// <exception cref="RouteInvalidArgumentException"></exception>
        public void AddRange(IList<Point> points) {
            for (int i = 0; i < points.Count - 1; i++) {
                if (!TilesClosely(points[i], points[i + 1])) {
                    throw new RouteInvalidArgumentException("Точки маршрута не представляют собой непрерывного пути.", points);
                }
            }
        }

        public Point Pop() {
            var out_value = route[0];
            route.RemoveAt(0);
            return out_value;
        }
        public void Overwrite(Route newRoute) => route = newRoute.route.ToList();
        internal void RemoveLast() {
            if (Empty) { throw new InvalidOperationException(); }
            route.RemoveLastItem();
        }


        public IEnumerator<Point> GetEnumerator() => route.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
