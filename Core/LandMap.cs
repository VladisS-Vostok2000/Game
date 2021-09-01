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



        /// <summary>
        /// <see cref="Landtile"/> на координатах X - вправо, Y - вниз.
        /// </summary>
        public Landtile this[Point location] => this[location.X, location.Y];
        /// /// <summary>
        /// <see cref="Landtile"/> на координатах X - вправо, Y - вниз.
        /// </summary>
        public Landtile this[int x, int y] {
            get {
                bool correct = CorrectIndexation(x, y);
                if (!correct) {
                    throw new ArgumentOutOfRangeException();
                }

                return landtiles[y, x];
            }
        }



        public LandMap(Landtile[,] landtiles) {
            if (landtiles is null) {
                throw new ArgumentNullException(nameof(landtiles));
            }
            if (landtiles.GetUpperBound(0) == 0 || landtiles.GetUpperBound(1) == 0) {
                throw new ArgumentException("Массив обязан иметь размер.");
            }

            this.landtiles = landtiles;
            Size = new Size(landtiles.GetUpperBound(1) + 1, landtiles.GetUpperBound(0) + 1);
        }



        public ReadOnlyDoubleDemensionArray<Landtile> AsReadOnly() => landtiles.AsReadOnly();


        public bool CorrectIndexation(Point location) => CorrectIndexation(location.X, location.Y);
        public bool CorrectIndexation(int x, int y) => x.IsInRange(0, Width - 1) && y.IsInRange(0, Height - 1);


        /// <summary>
        /// Возвращает <see cref="Landtile"/> на координатах X - право, Y - вниз.
        /// </summary>
        public bool TryGetLandtile(int x, int y, out Landtile landtile) {
            bool correct = CorrectIndexation(x, y);
            if (!correct) {
                landtile = default;
                return false;
            }

            landtile = this[y, x];
            return true;
        }


        public ColoredChar[,] AsColoredChars() {
            ColoredChar[,] outArray = new ColoredChar[Height, Width];
            for (int r = 0; r < Height; r++) {
                for (int c = 0; c < Width; c++) {
                    outArray[r, c] = landtiles[r, c].ColoredChar;
                }
            }
            return outArray;
        }

    }
}
