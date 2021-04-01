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
        private MapTile[] MapArray { get; }



        internal Map(string mapPath) {
            using (var streamReader = new StreamReader(mapPath)) {
                LengthX = int.Parse(streamReader.ReadLine());
                LengthY = int.Parse(streamReader.ReadLine());
                string charMap = ExistionMethods.ClearEmptySpaces(streamReader.ReadToEnd());
                MapArray = new MapTile[charMap.Length];
                for (int i = 0; i < charMap.Length; i++) {
                    MapArray[i] = new MapTile(charMap[i]);
                }
            }
        }



        internal MapTile this[int x, int y] => MapArray[y * LengthX + x];

    }
}
