using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using MyParsers;
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
        private const ConsoleColor selectedUnitRoutingColor = ConsoleColor.Magenta;
        public Unit SelectedUnit { get; private set; }
        public ICollection<Unit> Units { get; }
        public bool UnitSelected { get; private set; }
        public ICollection<Point> SelectedUnitAvailableRoutes { get; private set; }
        private IList<Point> SelectedUnitTempRoute;
        private float SelectedUnitTimeReserveTemp;

        public Team CurrentTeam { get; private set; }
        private int currentTeamIndex;



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
            CurrentTeam = rules.Teams[0];
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
                ChangeColors(outArray, SelectedUnit.GetRoute(), unitRouteColor);
                ChangeColors(outArray, SelectedUnitTempRoute, unitRouteColor);
                outArray[SelectedUnit.Location.X, SelectedUnit.Location.Y].Color = selectedUnitRoutingColor;
            }

            // Подсвеченный тайл.
            outArray[SelectedTileX, SelectedTileY].Color = selectedTileColor;

            return outArray;
            void ChangeColors(ConsoleImage[,] consoleImages, ICollection<Point> coordList, ConsoleColor color) {
                foreach (var coord in coordList) {
                    consoleImages[coord.X, coord.Y].Color = color;
                }
            }
        }


        #region Unit
        public Unit GetUnit(Point location) => Units.FirstOrDefault((unit) => unit.Location == location);
        public Unit GetUnit(in int x, in int y) => GetUnit(new Point(x, y));


        public void SelectUnit() {
            Unit unit = GetUnit(SelectedTileLocation);
            if (unit == null || unit.Team != CurrentTeam) return;

            SelectedUnit = unit;
            SelectedUnitTempRoute = new List<Point>();
            SelectedUnitTempRoute.AddRange(SelectedUnit.GetRoute());
            SelectedUnitTimeReserveTemp = SelectedUnit.TimeReserve;
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(SelectedUnitTimeReserveTemp);
            UnitSelected = true;
        }
        // FEATURE: добавить разделение времени на выход/вход в тайл.
        private List<Point> GetSelectedUnitAvailableRoutesPerTime(float timeReserve) {
            timeReserve = GetSelectedUnitRemainingTimeTemp(SelectedUnit, timeReserve);
            if (timeReserve <= 0) { return new List<Point>(); }
            Point unitTempLocation = GetSelectedUnitTempPosition();
            return FindUnitAvailableRoutesPerTime(unitTempLocation, SelectedUnit, timeReserve);
        }

        private float GetSelectedUnitRemainingTimeTemp(Unit unit, float timeReserve) {
            float remainingTime = unit.TimeReserve + timeReserve;
            var route = new List<Point>(unit.GetRoute());
            route.AddRange(SelectedUnitTempRoute);
            for (int i = 0; i < route.Count() && remainingTime > 0; i++) {
                Point way = route[i];
                Landtile land = Landtiles[way.X, way.Y];
                remainingTime -= UnitTimeSpentOnTile(land.Name, unit);
            }
            return remainingTime;
        }
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
            // BUG: приложение умирает при тайлах > 10.
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
            SelectedUnitTempRoute.Add(SelectedTileLocation);
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(SelectedUnitTimeReserveTemp);
        }
        public bool MaptileLocationAvailableForSelectedUnitTempMove(Point landtileLocation) =>
            MaptileReachableForSelectedUnit(landtileLocation) &&
            TileClosestToSelectedUnitTempPosition(landtileLocation);
        public bool MaptileIsSelectedUnitRoute(Point maptileLocation) => SelectedUnit.GetRoute().Contains(maptileLocation) || SelectedUnitTempRoute.Contains(maptileLocation);
        private bool TileClosestToSelectedUnitTempPosition(Point tile) {
            Point tempUnitPosition = GetSelectedUnitTempPosition();
            return ExtensionsMethods.TilesClosely(tile, tempUnitPosition);
        }
        private Point GetSelectedUnitTempPosition() {
            Point tempUnitPosition;
            if (SelectedUnitTempRoute.Count != 0) {
                tempUnitPosition = SelectedUnitTempRoute.Last();
            }
            else
            if (SelectedUnit.GetRoute().Count != 0) {
                tempUnitPosition = SelectedUnit.GetRoute().Last();
            }
            else {
                tempUnitPosition = SelectedUnit.Location;
            }
            return tempUnitPosition;
        }
        public bool MaptileReachableForSelectedUnit(Point tileLocation) => SelectedUnitAvailableRoutes.Contains(tileLocation);
        public void ConfirmSelectedUnitRoute() {
            SelectedUnit.GetRoute().AddRange(SelectedUnitTempRoute);
            SelectedUnit.TimeReserve = SelectedUnitTimeReserveTemp;
            UnselectUnit();
        }
        public void UnselectUnit() => UnitSelected = false;
        #endregion


        public void PassTurn() {
            UnselectUnit();
            currentTeamIndex = (currentTeamIndex + 1) % Rules.Teams.Count;
            CurrentTeam = Rules.Teams[currentTeamIndex];
        }
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
                if (unit.GetRoute().Count == 0) continue;

                while (unit.GetRoute().Count > 0 && unit.TimeReserve >= 0) {
                    Point nextRouteLocation = unit.GetRoute().First();
                    if (lockedTiles.Contains(nextRouteLocation)) break;
                    lockedTiles.Add(nextRouteLocation);

                    Landtile nextTile = Landtiles[nextRouteLocation.X, nextRouteLocation.Y];
                    float spentTimeToReachedTile = UnitTimeSpentOnTile(nextTile.Name, unit);
                    if (spentTimeToReachedTile > unit.TimeReserve) break;

                    unit.TimeReserve -= spentTimeToReachedTile;
                    unit.Location = nextRouteLocation;
                    unit.GetRoute().RemoveAt(0);
                }
            }
        }

    }
}
