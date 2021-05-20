using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undefinded;

namespace Game {
    public class Route : IEnumerable<Point> {
        private IList<Point> route = new List<Point>();
        public bool Empty => route.Count == 0;
        public Point Top => Empty? throw new InvalidOperationException() : route[0];



        public Route() { }
        /// <exception cref="ArgumentException"></exception>
        // What the hell is going here?
        public Route(IEnumerable<Point> sourse) { AddRange(sourse); }



        public Point this[int index] => route[index];



        public void Add(Point way) {
            if (Empty) {
                route.Add(way);
                return;
            }

            Point unitTempLocation = route.Last();
            bool valid = ExtensionsMethods.TilesClosely(unitTempLocation, way);
            if (!valid) { throw new ArgumentException("Путь находится не в ближайшей конечной точке маршрута."); }

            route.Add(way);
        }
        public Point Pop() {
            var out_value = route[0];
            route.RemoveAt(0);
            return out_value;
        }
        public void AddRange(IEnumerable<Point> points) {
            foreach (var point in points) {
                Add(point);
            }
        }
        public void Overwrite(Route newRoute) => route = newRoute.route.ToList();
        internal void RemoveLast() {
            if (Empty) { throw new InvalidOperationException(); }
            route.RemoveLast();
        }


        public IEnumerator<Point> GetEnumerator() => route.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
