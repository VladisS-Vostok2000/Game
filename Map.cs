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
    public sealed class Map {
        private const float speedPerTile = 20;
        private const float tirnTimeTick = 1;


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
        private IList<Point> SelectedUnitTempRoute { get; set; }
        private float UnitTimeReservePerTurn = 5;

        public Team CurrentTeam { get; private set; }
        private int currentTeamIndex;



        public MaptileInfo this[int x, int y] {
            get {
                // REFACTORING: Повторяющийся код.
                if (UnitSelected) {
                    return new MaptileInfo(
                        Landtiles[x, y],
                        new Point(x, y),
                        GetUnitOrNull(x, y),
                        MaptileReachableForSelectedUnit(new Point(x, y)),
                        MaptileLocationAvailableForSelectedUnitTempMove(new Point(x, y))
                    );
                }
                else {
                    return new MaptileInfo(
                        Landtiles[x, y],
                        new Point(x, y),
                        GetUnitOrNull(x, y),
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
            Units = ExtractValidUnits(units).ToList();
            CurrentTeam = rules.Teams[0];
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
            void ChangeColors(ConsoleImage[,] consoleImages, IEnumerable<Point> coordList, ConsoleColor color) {
                foreach (var coord in coordList) {
                    consoleImages[coord.X, coord.Y].Color = color;
                }
            }
        }


        #region Unit
        public Unit GetUnitOrNull(Point location) => Units.FirstOrDefault((unit) => unit.Location == location);
        public Unit GetUnitOrNull(in int x, in int y) => GetUnitOrNull(new Point(x, y));


        public void SelectUnit() {
            Unit unit = GetUnitOrNull(SelectedTileLocation);
            if (unit == null || unit.Team != CurrentTeam) { return; }

            SelectedUnit = unit;
            UnitSelected = true;
            SelectedUnitTempRoute = new List<Point>();
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(UnitTimeReservePerTurn);
        }
        // FEATURE: добавить разделение времени на выход/вход в тайл.
        private List<Point> GetSelectedUnitAvailableRoutesPerTime(float timeReserve) {
            timeReserve = GetSelectedUnitRemainingTempTime(SelectedUnit, timeReserve);
            if (timeReserve <= 0) { return new List<Point>(); }
            Point unitTempLocation = GetSelectedUnitLastRoutePosition();
            return FindUnitAvailableRoutesPerTime(unitTempLocation, SelectedUnit, timeReserve);
        }
        private float GetSelectedUnitRemainingTempTime(Unit unit, float timeReserve) {
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
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X + 1, unitStartLocation.Y, unit, timeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X - 1, unitStartLocation.Y, unit, timeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X, unitStartLocation.Y + 1, unit, timeReserve));
            outList.AddRange(GetUnitAvailableRoutesPerTimeWithTile(unitStartLocation.X, unitStartLocation.Y - 1, unit, timeReserve));
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
            bool tileExists = TryGetLandtile(x, y, out Landtile landtile);
            if (!tileExists) {
                return outList;
            }

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
        private bool TryGetLandtile(int landtileX, int landtileY, out Landtile landtile) {
            bool correctIndexation = landtileX.IsInRange(0, LengthX - 1) && landtileY.IsInRange(0, LengthY - 1);
            landtile = correctIndexation ? Landtiles[landtileX, landtileY] : default;
            return correctIndexation;
        }

        public static float UnitTimeSpentOnTile(string landtileName, Unit unit) => speedPerTile / unit.CalculateSpeedOnLandtile(landtileName);

        public void ConfrimSelectedUnitRoute() {
            if (!UnitSelected
                || !MaptileLocationAvailableForSelectedUnitTempMove(SelectedTileLocation)
                || MaptileIsSelectedUnitRoute(SelectedTileLocation)) {
                return;
            }

            string newPositionLandtileName = SelectedTile.Land.Name;
            SelectedUnitTempRoute.Add(SelectedTileLocation);
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(UnitTimeReservePerTurn);
        }

        public bool MaptileLocationAvailableForSelectedUnitTempMove(Point landtileLocation) =>
            MaptileReachableForSelectedUnit(landtileLocation) &&
            TileClosestToSelectedUnitTempPosition(landtileLocation);

        public bool MaptileIsSelectedUnitRoute(Point maptileLocation) => SelectedUnit.GetRoute().Contains(maptileLocation) || SelectedUnitTempRoute.Contains(maptileLocation);
        private bool TileClosestToSelectedUnitTempPosition(Point tile) {
            Point tempUnitPosition = GetSelectedUnitLastRoutePosition();
            return ExtensionsMethods.TilesClosely(tile, tempUnitPosition);
        }
        
        public Point GetSelectedUnitLastRoutePosition() {
            if (!UnitSelected) { throw new InvalidOperationException(); }

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
            SelectedUnit.AppendRoute(SelectedUnitTempRoute);
            UnselectUnit();
        }
        
        public void UnselectUnit() => UnitSelected = false;

        public void DeleteSelectedUnitLastWay() {
            if (!UnitSelected) { return; }

            if (!SelectedUnitTempRoute.Empty()) {
                SelectedUnitTempRoute.RemoveLast();
                return;
            }

            bool removedSuccessfully = SelectedUnit.TryRemoveLastWay();
            if (!removedSuccessfully) { return; }

            if (SelectedUnit.GetRoute().Empty()) { SelectedUnit.TimeReserve = 0; }
        }
        #endregion


        public void PassTurn() {
            UnselectUnit();
            currentTeamIndex = (currentTeamIndex + 1) % Rules.Teams.Count;
            CurrentTeam = Rules.Teams[currentTeamIndex];
        }
        public void MakeTurn() {
            MoveUnits(tirnTimeTick);
        }
        private void MoveUnits(float allotedTime) {
            // BUG: список заблокированных маршрутов не хранится, т.к. иначе не сможет
            // заработать, однако юнит, передвигающуюся на локацию, на который передвигается
            // другой, при перемешивании списка уже может быть не первым претендентом на неё.
            // В таком случае у него будет потерянный избыток времени, а если список будет "удачно"
            // перемешиваться потоянно, локация никогда не будет достигнута кем-то из них.
            var lockedTiles = Units.Select(unit => unit.Location).ToList();

            foreach (var unit in Units) {
                if (unit.GetRoute().Count == 0) { continue; }
                unit.TimeReserve += allotedTime;

                do {
                    Point nextWayLocation = unit.NextLocation;
                    if (lockedTiles.Contains(nextWayLocation)) {
                        unit.TimeReserve = 0;
                        break;
                    }

                    lockedTiles.Add(nextWayLocation);

                    Landtile nextTile = Landtiles[nextWayLocation.X, nextWayLocation.Y];
                    float spentTimeToReachedTile = UnitTimeSpentOnTile(nextTile.Name, unit);
                    bool enoughtTime = unit.TimeReserve >= spentTimeToReachedTile;
                    if (!enoughtTime) { break; }

                    unit.TimeReserve -= spentTimeToReachedTile;
                    unit.Move();

                    if (unit.GetRoute().Empty()) {
                        unit.TimeReserve = 0;
                        break;
                    }
                } while (unit.TimeReserve > 0);
            }
        }


        private IEnumerable<Unit> ExtractValidUnits(IEnumerable<Unit> units) => units.Distinct(new UnitLocationEqualsComparer());

    }
}
