using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.BasicTypesLibrary {
    public sealed class ReadOnlyArray<T> : IReadOnlyList<T> {
        private T[] array;
        public int Count => array.Length;



        public ReadOnlyArray(T[] sourse) {
            array = sourse != null ? sourse : throw new ArgumentNullException($"{nameof(sourse)}");
        }



        public T this[int index] {
            get {
                try { return array[index]; }
                catch (IndexOutOfRangeException) { throw; }
            }
        }



        public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)array.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => array.GetEnumerator();

    }
}
