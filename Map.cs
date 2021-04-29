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
    // FEATURE: добавить смену хода.
    public sealed class Map {
        private const float speedPerTile = 20;
        private const float timePerTurn = 5;
        private const float turnTimeDivision = 1;


        public int LengthX => Landtiles.GetUpperBound(0) + 1;
        public int LengthY => Landtiles.GetUpperBound(1) + 1;
        public int Square => LengthX * LengthY;


        public Landtile[,] Landtiles { get; }
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
        public MaptileInfo SelectedTile => this[SelectedTileX, SelectedTileY];


        private const ConsoleColor unitAvailableRoutesColor = ConsoleColor.Cyan;
        private const ConsoleColor unitRouteColor = ConsoleColor.Red;
        private const ConsoleColor selectedUnitRouting = ConsoleColor.Magenta;
        public Unit SelectedUnit { get; private set; }
        public ICollection<Unit> Units { get; }
        public bool UnitSelected { get; private set; }
        public ICollection<Point> SelectedUnitAvailableRoutes { get; private set; }
        private IList<Point> SelectedUnitRouteTemp;
        private float SelectedUnitTimeReserveTemp;



        public MaptileInfo this[int x, int y] {
            get {
                // REFACTORING: Повторяющийся код.
                if (UnitSelected) {
                    return new MaptileInfo(
                        Landtiles[x, y],
                        GetUnit(x, y),
                        MaptileReachableForSelectedUnit(new Point(x, y)),
                        MaptileLocationAvailableForSelectedUnitTempMove(new Point(x, y)),
                        MaptileIsSelectedUnitRoute(new Point(x, y))
                    );
                }
                else {
                    return new MaptileInfo(
                        Landtiles[x, y],
                        GetUnit(x, y),
                        false,
                        false,
                        false
                    );
                }
            }
        }
        public MaptileInfo this[Point point] => this[point.X, point.Y];



        public Map(Landtile[,] landtiles, Rules rules, IList<Unit> units) {
            Landtiles = landtiles;
            Rules = rules;
            Units = units;

            foreach (var unit in Units) {
                unit.TimeReserve = timePerTurn;
            }
        }



        public ConsoleImage[,] Visualize() {
            var outArray = new ConsoleImage[LengthX, LengthY];

            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outArray[c, r] = this[c, r].ToConsoleImage();
                }
            }

            if (UnitSelected) {
                ChangeColors(outArray, SelectedUnitAvailableRoutes, unitAvailableRoutesColor);
                ChangeColors(outArray, SelectedUnit.Route, unitRouteColor);
                ChangeColors(outArray, SelectedUnitRouteTemp, unitRouteColor);
                outArray[SelectedUnit.Location.X, SelectedUnit.Location.Y].Color = selectedUnitRouting;
            }

            // Подсвеченный тайл.
            outArray[SelectedTileX, SelectedTileY] = new ConsoleImage(SelectedTile.ToConsoleImage().Char, selectedTileColor);

            return outArray;
            void ChangeColors(ConsoleImage[,] consoleImages, ICollection<Point> coordList, ConsoleColor color) {
                foreach (var coord in coordList) {
                    consoleImages[coord.X, coord.Y].Color = color;
                }
            }
        }


        #region Unit
        public Unit GetUnit(Point location) => Units.FirstOrDefault((Unit unit) => unit.Location == location);
        public Unit GetUnit(int x, int y) => GetUnit(new Point(x, y));


        public void SelectUnit() {
            Unit unit = GetUnit(SelectedTileLocation);
            if (unit == null) {
                return;
            }

            SelectedUnit = unit;
            SelectedUnit.Location = SelectedTileLocation;
            SelectedUnitRouteTemp = new List<Point>();
            SelectedUnitRouteTemp.AddRange(SelectedUnit.Route);
            SelectedUnitTimeReserveTemp = SelectedUnit.TimeReserve;
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(SelectedUnitTimeReserveTemp);
            UnitSelected = true;
        }
        // FEATURE: добавить разделение времени на выход/вход в тайл.
        private List<Point> GetSelectedUnitAvailableRoutesPerTime(float timeReserve) =>
            FindUnitAvailableRoutesPerTime(GetSelectedUnitTempPosition(), SelectedUnit, timeReserve);
        private List<Point> FindUnitAvailableRoutesPerTime(in Point unitStartLocation, Unit unit, float timeReserve) {
            var outList = new List<Point>();
            // REFACTORING: подключить многопоточность?
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X + 1, unitStartLocation.Y, SelectedUnit, timeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X - 1, unitStartLocation.Y, SelectedUnit, timeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X, unitStartLocation.Y + 1, SelectedUnit, timeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X, unitStartLocation.Y - 1, SelectedUnit, timeReserve));
            // REFACTORING: такое себе решение от дубликатов.
            foreach (var location in outList) {
                if (!outList.Contains(location)) {
                    outList.Add(location);
                }
            }
            return outList;
        }
        private List<Point> GetUnitAvailableRoutesPerTimeWithTile(in int x, in int y, Unit unit, float unitTimeReserve) {
            // FEATURE: теоретически, можно сделать это эффективнее, если
            // рассчитывать не все тайлы по нескольку раз подряд, а делать
            // это итеративно и выбирать тайлы с наибольшим запасом времени.
            // Это довольно сложно реализовать, а также потребует память
            // и лишит преимущества простого распараллеливания.
            var outList = new List<Point>();
            bool tileExists = TryGetTile(x, y, out MaptileInfo landtileInfo);
            if (!tileExists) {
                return outList;
            }

            Landtile landtile = landtileInfo.Land;
            string landtileName = landtile.Name;
            float timeSpent = UnitTimeSpentOnTile(landtileName, unit);
            if (timeSpent > unitTimeReserve) {
                return outList;
            }
            // REFACTORING: такое себе решение от дубликатов.
            if (!outList.Contains(new Point(x, y))) {
                outList.Add(new Point(x, y));
            }
            unitTimeReserve -= timeSpent;
            // REFACTORING: подключить многопоточность?
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(x + 1, y, unit, unitTimeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(x - 1, y, unit, unitTimeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(x, y + 1, unit, unitTimeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(x, y - 1, unit, unitTimeReserve));
            return outList;
        }
        private bool TryGetTile(Point landtileLocation, out MaptileInfo landtile) => TryGetTile(landtileLocation.X, landtileLocation.Y, out landtile);
        private bool TryGetTile(int landtileX, int landtileY, out MaptileInfo landtile) {
            bool correctIndexation = landtileX.IsInRange(0, LengthX - 1) && landtileY.IsInRange(0, LengthY - 1);
            landtile = correctIndexation ? this[landtileX, landtileY] : default;
            return correctIndexation;
        }

        public static float UnitTimeSpentOnTile(string landtileName, Unit unit) => speedPerTile / unit.CalculateSpeedOnLandtile(landtileName);

        public void AddSelectedUnitRoute() {
            if (!UnitSelected
                || !MaptileLocationAvailableForSelectedUnitTempMove(SelectedTileLocation)
                || MaptileIsSelectedUnitRoute(SelectedTileLocation)) {
                return;
            }

            string newPositionLandtileName = SelectedTile.Land.Name;
            float timeSpent = UnitTimeSpentOnTile(newPositionLandtileName, SelectedUnit);
            SelectedUnitTimeReserveTemp -= timeSpent;
            SelectedUnitRouteTemp.Add(SelectedTileLocation);
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(SelectedUnitTimeReserveTemp);
        }
        public bool MaptileLocationAvailableForSelectedUnitTempMove(Point landtileLocation) =>
            MaptileReachableForSelectedUnit(landtileLocation) &&
            TileClosestToSelectedUnitTempPosition(landtileLocation);
        public bool MaptileIsSelectedUnitRoute(Point maptileLocation) => SelectedUnit.Route.Contains(maptileLocation) || SelectedUnitRouteTemp.Contains(maptileLocation);
        private bool TileClosestToSelectedUnitTempPosition(Point tile) {
            Point tempUnitPosition = GetSelectedUnitTempPosition();
            return (Math.Abs(tile.X - tempUnitPosition.X) == 1 && tile.Y == tempUnitPosition.Y)
                || (Math.Abs(tile.Y - tempUnitPosition.Y) == 1 && tile.X == tempUnitPosition.X);
        }
        private Point GetSelectedUnitTempPosition() {
            Point tempUnitPosition;
            if (SelectedUnitRouteTemp.Count != 0) {
                tempUnitPosition = SelectedUnitRouteTemp.Last();
            }
            else
            if (SelectedUnit.Route.Count != 0) {
                tempUnitPosition = SelectedUnit.Route.Last();
            }
            else {
                tempUnitPosition = SelectedUnit.Location;
            }
            return tempUnitPosition;
        }
        public bool MaptileReachableForSelectedUnit(Point tileLocation) => SelectedUnitAvailableRoutes.Contains(tileLocation);
        public void ConfirmSelectedUnitRoute() {
            SelectedUnit.Route.AddRange(SelectedUnitRouteTemp);
            SelectedUnit.TimeReserve = SelectedUnitTimeReserveTemp;
            UnselectUnit();
        }
        public void UnselectUnit() => UnitSelected = false;
        #endregion


        public void MakeTurn() {
            for (float i = 0; i < timePerTurn; i += turnTimeDivision) {
                MoveUnits();
            }
            foreach (var unit in Units) {
                unit.TimeReserve = 5;
            }
        }
        private void MoveUnits() {
            var lockedTiles = Units.Select(unit => unit.Location).ToList();

            foreach (var unit in Units) {
                unit.TimeReserve += turnTimeDivision;
                if (unit.Route.Count == 0) continue;

                while (unit.Route.Count > 0 && unit.TimeReserve >= 0) {
                    Point nextRouteLocation = unit.Route.First();
                    if (lockedTiles.Contains(nextRouteLocation)) break;
                    lockedTiles.Add(nextRouteLocation);

                    Landtile nextTile = Landtiles[nextRouteLocation.X, nextRouteLocation.Y];
                    float spentTimeToReachedTile = UnitTimeSpentOnTile(nextTile.Name, unit);
                    if (spentTimeToReachedTile > unit.TimeReserve) break;

                    unit.TimeReserve -= spentTimeToReachedTile;
                    unit.Location = nextRouteLocation;
                    unit.Route.RemoveAt(0);
                }
            }
        }

    }
}
