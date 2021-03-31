using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Game {
    internal class Map {
        internal int LengthX { get; }
        internal int LengthY { get; }
        private string MapArray { get; }



        internal Map(string mapPath) {
            using (var streamReader = new StreamReader(mapPath)) {
                LengthX = int.Parse(streamReader.ReadLine());
                LengthY = int.Parse(streamReader.ReadLine());
                MapArray = ExistionMethods.ClearEmptySpaces(streamReader.ReadToEnd());
            }
        }



        internal char this[int x, int y] => MapArray[y * LengthX + x];

    }
}
