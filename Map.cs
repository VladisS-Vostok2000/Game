using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using IniParser;
using Undefinded;

namespace Game {
    public sealed class Map {
        private const float speedPerTile = 20;
        private const float timePerTurn = 5;


        public int LengthX => LandtilesMap.GetUpperBound(0) + 1;
        public int LengthY => LandtilesMap.GetUpperBound(1) + 1;
        public int Square => LengthX * LengthY;
        public Unit[,] Units { get; }


        public Landtile[,] LandtilesMap { get; }
        public Rules Rules { get; }


        public Point SelectedTileLocation {
            get => new Point(SelectedTileX, SelectedTileY);
            set {
                SelectedTileX = value.X;
                SelectedTileY = value.Y;
            }
        }
        private int selectedTileX;
        private int selectedTileY;
        public int SelectedTileX {
            get => selectedTileX;
            set => selectedTileX = value.InRange(0, LengthX);
        }
        public int SelectedTileY {
            get => selectedTileY;
            set => selectedTileY = value.InRange(0, LengthY);
        }
        private static readonly ConsoleColor selectedTileColor = ConsoleColor.Yellow;
        public MapTileInfo SelectedTile => this[SelectedTileX, SelectedTileY];


        public Unit SelectedUnit { get; private set; }
        private int SelectedUnitX;
        private int SelectedUnitY;
        public Point SelectedUnitLocation {
            get => new Point(SelectedUnitX, SelectedUnitY);
            set {
                SelectedUnitX = value.X;
                SelectedUnitY = value.Y;
            }
        }
        public bool UnitSelected { get; private set; }
        public IList<Point> SelectedUnitAvailableRoutes { get; private set; }
        private const ConsoleColor unitRoadmapColor = ConsoleColor.Cyan;
        private const ConsoleColor unitRoadmapPathColor = ConsoleColor.Red;



        public MapTileInfo this[int x, int y] {
            get {
                if (UnitSelected) {
                    return new MapTileInfo(
                        LandtilesMap[x, y],
                        Units[x, y],
                        SelectedTileReachableForSelectedUnit(),
                        SelectedTileClosestToSelectedUnit(),
                        SelectedTileIsUnitRoute()
                    );
                }
                else {
                    return new MapTileInfo(
                        LandtilesMap[x, y],
                        Units[x, y],
                        false,
                        false,
                        false
                    );
                }
            }
        }
        public MapTileInfo this[Point point] => this[point.X, point.Y];



        public Map(Landtile[,] landtiles, Rules rules, Unit[,] units) {
            LandtilesMap = landtiles;
            Rules = rules;
            Units = units;
        }



        public ConsoleImage[,] Visualize() {
            if (UnitSelected) {
                BuildSelectedUnitRoadmap();
            }

            var outArray = new ConsoleImage[LengthX, LengthY];

            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outArray[c, r] = this[c, r].ToConsoleImage();
                }
            }

            // TODO: можно выделить функции ниже в локальные.
            if (UnitSelected) {
                Unit unit = SelectedUnit;
                // Подсветка доступных для передвижения тайлов выделенного юнита.
                foreach (var unitWay in SelectedUnitAvailableRoutes) {
                    outArray[unitWay.X, unitWay.Y] = new ConsoleImage(this[unitWay].ToConsoleImage().Char, unitRoadmapColor);
                }

                // Подсветка намеченного пути.
                IList<Point> unitPathPoints = unit.UnitPath;
                foreach (var unitPathPoint in unitPathPoints) {
                    var tileImage = this[unitPathPoint].ToConsoleImage();
                    outArray[unitPathPoint.X, unitPathPoint.Y] = new ConsoleImage(tileImage.Char, unitRoadmapPathColor);
                }
            }


            // Подсвеченный тайл.
            outArray[SelectedTileX, SelectedTileY] = new ConsoleImage(SelectedTile.ToConsoleImage().Char, selectedTileColor);

            return outArray;
        }
        //public Unit GetUnit(int x, int y) => Units[x, y];
        //public Landtile Getlandtile(int x, int y) => LandtilesMap[x, y];
        //public ConsoleImage GetConsoleImage(int x, int y) => Units[x, y]?.ConsoleImage ?? LandtilesMap[x, y].ConsoleImage;
        //public ConsoleImage GetConsoleImage(Point location) => GetConsoleImage(location.X, location.Y);


        public void SelectUnit() {
            Unit unit = Units[SelectedTileX, SelectedTileY];
            if (unit == null) {
                return;
            }

            SelectedUnit = unit;
            SelectedUnitLocation = SelectedTileLocation;
            UnitSelected = true;
            SelectedUnit.UnitPath = new List<Point>() { SelectedUnitLocation };
            BuildSelectedUnitRoadmap();
        }


        // TODO: добавить разделение времени на выход/вход в тайл.
        private void BuildSelectedUnitRoadmap() {
            SelectedUnitAvailableRoutes = new List<Point>();
            Unit unit = SelectedUnit;
            Point unitLocation = unit.UnitPath.Last();
            float reservedTime = unit.ReservedTime;
            // TODO: подключить многопоточность?
            FindAvailableRoutes(unitLocation.X + 1, unitLocation.Y, reservedTime);
            FindAvailableRoutes(unitLocation.X - 1, unitLocation.Y, reservedTime);
            FindAvailableRoutes(unitLocation.X, unitLocation.Y + 1, reservedTime);
            FindAvailableRoutes(unitLocation.X, unitLocation.Y - 1, reservedTime);
        }
        private void FindAvailableRoutes(int x, int y, double timeReserve) {
            // TODO: теоретически, можно сделать это эффективнее, если
            // рассчитывать не все тайлы по нескольку раз подряд, а делать
            // это итеративно и выбирать тайлы с наибольшим запасом времени.
            // Это довольно сложно реализовать, а также потребует память
            // и лишит преимущество простого распараллеливания.
            bool tileExists = TryGetTile(x, y, out MapTileInfo landtileInfo);
            if (!tileExists) {
                return;
            }

            Landtile landtile = landtileInfo.Land;
            string landtileName = landtile.Name;
            float timeSpent = SelectedUnitTimeSpentOnTile(landtileName);
            if (timeSpent > timeReserve) {
                return;
            }

            SelectedUnitAvailableRoutes.Add(new Point(x, y));
            timeReserve -= timeSpent;
            // TODO: подключить многопоточность?
            FindAvailableRoutes(x + 1, y, timeReserve);
            FindAvailableRoutes(x - 1, y, timeReserve);
            FindAvailableRoutes(x, y + 1, timeReserve);
            FindAvailableRoutes(x, y - 1, timeReserve);
        }
        private float SelectedUnitTimeSpentOnTile(string landtileName) => speedPerTile / SelectedUnit.CalculateSpeed(landtileName);
        private bool TryGetTile(int landtileX, int landtileY, out MapTileInfo landtile) {
            bool correctIndexation = landtileX.IsInRange(0, LengthX - 1) && landtileY.IsInRange(0, LengthY - 1);
            landtile = correctIndexation ? this[landtileX, landtileY] : default;
            return correctIndexation;
        }

        public void UnselectUnit() => UnitSelected = false;

        public void AddUnitPath() {
            if (!UnitSelected) {
                return;
            }

            // TODO: стоит сделать методы "статическими"? Передоз полей.
            Point currentUnitPosition = SelectedUnit.UnitPath.Last();
            Point newUnitPosition = SelectedTileLocation;
            bool routeAvailable = SelectedTileReachableForSelectedUnit() && SelectedTileClosestToSelectedUnit();
            if (!routeAvailable) {
                return;
            }

            string newPositionLandtileName = this[newUnitPosition].Land.Name;
            float timeSpent = SelectedUnitTimeSpentOnTile(newPositionLandtileName);
            SelectedUnit.UnitPath.Add(SelectedTileLocation);
            SelectedUnit.ReservedTime -= timeSpent;
        }
        public bool SelectedTileClosestToSelectedUnit() => (Math.Abs(SelectedTileX - SelectedUnit.UnitPath.Last().X) == 1 && SelectedTileY == SelectedUnit.UnitPath.Last().Y) ||
                Math.Abs(SelectedTileY - SelectedUnit.UnitPath.Last().Y) == 1 && SelectedTileX == SelectedUnit.UnitPath.Last().X;
        public bool SelectedTileReachableForSelectedUnit() => SelectedUnitAvailableRoutes.Contains(new Point(SelectedTileX, SelectedTileY));
        public bool SelectedTileIsUnitRoute() => SelectedUnit.UnitPath.Contains(new Point(SelectedTileX, SelectedTileY));
        public bool SelectedTileAvailableForUnitMove() => SelectedTileReachableForSelectedUnit() && SelectedTileClosestToSelectedUnit();

    }
}
