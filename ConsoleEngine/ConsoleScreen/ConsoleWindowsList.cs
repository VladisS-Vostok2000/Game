using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsoleWindowsList : IEnumerable<ColoredCharPanel> {
        public int Count => list.Count;



        private List<ColoredCharPanel> list;



        public ConsoleWindowsList() => list = new List<ColoredCharPanel>();



        public void Add(ColoredCharPanel consoleWindow) => list.Add(consoleWindow);
        public void RemoveAt(int index) {
            try { list.RemoveAt(index); }
            catch (ArgumentOutOfRangeException) { throw; }
        }


        #region IEnumerable
        public IEnumerator<ColoredCharPanel> GetEnumerator() => ((IEnumerable<ColoredCharPanel>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();
        #endregion

    }
}
