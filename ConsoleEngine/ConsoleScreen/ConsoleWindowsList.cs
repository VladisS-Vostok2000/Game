using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine {
    public class ConsoleWindowsList : IEnumerable<ConsoleWindow> {
        public int Count => list.Count;



        private List<ConsoleWindow> list;



        public ConsoleWindowsList() => list = new List<ConsoleWindow>();



        public void Add(ConsoleWindow consoleWindow) => list.Add(consoleWindow);
        public void RemoveAt(int index) {
            try { list.RemoveAt(index); }
            catch (ArgumentOutOfRangeException) { throw; }
        }


        #region IEnumerable
        public IEnumerator<ConsoleWindow> GetEnumerator() => ((IEnumerable<ConsoleWindow>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();
        #endregion

    }
}
