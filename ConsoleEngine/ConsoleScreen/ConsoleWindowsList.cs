using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsoleWindowsList : IEnumerable<ColoredCharWindow> {
        public int Count => list.Count;



        private List<ColoredCharWindow> list;



        public ConsoleWindowsList() => list = new List<ColoredCharWindow>();



        public void Add(ColoredCharWindow consoleWindow) => list.Add(consoleWindow);
        public void RemoveAt(int index) {
            try { list.RemoveAt(index); }
            catch (ArgumentOutOfRangeException) { throw; }
        }


        #region IEnumerable
        public IEnumerator<ColoredCharWindow> GetEnumerator() => ((IEnumerable<ColoredCharWindow>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();
        #endregion

    }
}
