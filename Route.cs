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



        public Point this[int index] => route[index];



        public void Add(Point way) {
            if (Empty) {
                route.Add(way);
                return;
            }

            bool valid = ExtensionsMethods.TilesClosely(route.Last(), way);
            if (!valid) { 
                throw new InvalidOperationException(); }

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
        public void Overwrite(Route newRoute) => route = newRoute.route;


        public IEnumerator<Point> GetEnumerator() => route.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
