using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.BasicTypesLibrary {
    public sealed class ReadOnlyDoubleDemensionArray<T> {
        private T[,] array;



        public T this[int x, int y] => array[x, y];



        public ReadOnlyDoubleDemensionArray(T[,] array) => this.array = array;

    }
}
