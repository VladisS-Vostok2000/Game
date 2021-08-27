using Game.BasicTypesLibrary;
using Game.BasicTypesLibrary.ExtensionMethods;
using Game.ColoredCharsEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.Core {
    public sealed class LandMap {
        public Size Size { get; }
        public int Width => Size.Width;
        public int Height => Size.Height;

        private Landtile[,] landtiles;



        public Landtile this[Point location] => this[location.X, location.Y];
        public Landtile this[int x, int y] {
            get {
                bool correct = CorrectIndexation(x, y);
                if (!correct) {
                    throw new ArgumentOutOfRangeException();
                }

                return landtiles[x, y];
            }
        }



        public LandMap(Landtile[,] landtiles) {
            if (landtiles is null) {
                throw new ArgumentNullException(nameof(landtiles));
            }
            if (landtiles.GetUpperBound(0) == 0 || landtiles.GetUpperBound(0) == 0) {
                throw new ArgumentException("Массив обязан иметь размер.");
            }

            this.landtiles = landtiles;
            Size = new Size(landtiles.GetUpperBound(1), landtiles.GetUpperBound(0));
        }



        public ReadOnlyDoubleDemensionArray<Landtile> AsReadOnly() => landtiles.AsReadOnly();


        public bool CorrectIndexation(Point location) => CorrectIndexation(location.X, location.Y);
        public bool CorrectIndexation(int x, int y) => x.IsInRange(0, Width - 1) && y.IsInRange(0, Height - 1);


        public bool TryGetLandtile(int x, int y, out Landtile landtile) {
            bool correct = CorrectIndexation(x, y);
            if (!correct) {
                landtile = default;
                return false;
            }

            landtile = this[x, y];
            return true;
        }

    }
}
