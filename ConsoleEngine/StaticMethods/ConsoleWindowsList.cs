using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsoleWindowsList : IEnumerable<ConsolePanel> {
        public int Count => list.Count;



        private List<ConsolePanel> list;



        public ConsoleWindowsList() => list = new List<ConsolePanel>();



        public void Add(ConsolePanel consoleWindow) => list.Add(consoleWindow);
        public void RemoveAt(int index) {
            try { list.RemoveAt(index); }
            catch (ArgumentOutOfRangeException) { throw; }
        }


        #region IEnumerable
        public IEnumerator<ConsolePanel> GetEnumerator() => ((IEnumerable<ConsolePanel>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();
        #endregion

    }
}
