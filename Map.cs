using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
namespace Game {
    internal class Map {
        internal int LengthX { get; }
        internal int LengthY { get; }
        internal int Length => LengthX * LengthY;
        
        internal Point SelectedTileLocation {
            get => new Point(SelectedTileX, SelectedTileY);
            set {
                SelectedTileX = value.X;
                SelectedTileY = value.Y;
            }
        }
        private int selectedTileX;
        private int selectedTileY;
        internal int SelectedTileX {
            get => selectedTileX;
            set => selectedTileX = value.InRange(0, LengthX);
        }
        internal int SelectedTileY {
            get => selectedTileY;
            set => selectedTileY = value.InRange(0, LengthY);
        }
        private static readonly ConsoleColor selectedTileColor = ConsoleColor.Yellow;

        internal MapTileInfo SelectedTile => this[SelectedTileX, SelectedTileY];

        internal LandTile[,] LandTiles { get; }
        internal Unit[,] Units { get; }



        internal Map(string mapPath) {
            using (var streamReader = new StreamReader(mapPath)) {
                LengthX = int.Parse(streamReader.ReadLine());
                LengthY = int.Parse(streamReader.ReadLine());
                string charMap = streamReader.ReadToEnd().ClearEmptySpaces();
                LandTiles = new LandTile[LengthX, LengthY];
                int i = 0;
                for (int r = 0; r < LengthY; r++) {
                    for (int c = 0; c < LengthX; c++) {
                        LandTile.LandTileTypes landTileType = LandTile.CharToLandTileType(charMap[i++]);
                        if (landTileType == LandTile.LandTileTypes.None) {
                            throw new Exception();
                        }

                        LandTiles[c, r] = new LandTile(landTileType);
                    }
                }
                Units = new Unit[LengthX, LengthY];
            }
        }



        internal MapTileInfo this[int x, int y] => new MapTileInfo(LandTiles[x, y], Units[x, y]);



        internal ConsoleImage[,] ToConsoleImages() {
            ConsoleImage[,] outArray = new ConsoleImage[LengthX, LengthY];
            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outArray[c, r] = Units[c, r]?.ConsoleImage ?? LandTiles[c, r].ConsoleImage;
                }
            }
            outArray[SelectedTileLocation.X, SelectedTileLocation.Y].Color = selectedTileColor;
            return outArray;
        }
        internal Unit GetUnit(int x, int y) => Units[x, y];
        internal LandTile GetLandTile(int x, int y) => LandTiles[x, y];
        internal ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandTiles[x, y].ConsoleImage;
        internal ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);

    }
}
