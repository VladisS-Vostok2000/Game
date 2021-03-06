using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Game.Parser;
using Game.ColoredCharsEngine;
using System.Windows.Forms;
using Game.BasicTypesLibrary;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace Game.Core {
    public sealed class GameMap {
        private const float speedPerTile = 20;
        private const float turnTimeTick = 1;


        public int Width => LandMap.Width;
        public int Height => LandMap.Height;
        public int Square => Width * Height;
        public Size Size => LandMap.Size;


        public LandMap LandMap { get; } 
        public Rules Rules { get; }


        public Point SelectedTileLocation {
            get => new Point(selectedTileX, selectedTileY);
            set {
                selectedTileX = value.X.ToRange(0, Width);
                selectedTileY = value.Y.ToRange(0, Height);
            }
        }
        private int selectedTileX;
        private int selectedTileY;
        public int SelectedTileX {
            get => selectedTileX;
            set {
                selectedTileX = value.ToRange(0, Width);
            }
        }
        public int SelectedTileY {
            get => selectedTileY;
            set => selectedTileY = value.ToRange(0, Height);
        }
        private static readonly ConsoleColor selectedTileColor = ConsoleColor.Yellow;
        public MaptileInfo SelectedTile => this[SelectedTileX, SelectedTileY];


        private const ConsoleColor unitAvailableRoutesColor = ConsoleColor.Cyan;
        private const ConsoleColor unitRouteColor = ConsoleColor.Red;
        private const ConsoleColor selectedUnitRoutingColor = ConsoleColor.Magenta;
        public Unit SelectedUnit { get; private set; }
        public ICollection<Unit> Units { get; }
        public bool UnitSelected { get; private set; }
        private ICollection<Point> selectedUnitAvailableRoutes { get; set; }
        public IReadOnlyCollection<Point> SelectedUnitAvailableRoutes;
        private IList<Point> SelectedUnitTempRoute { get; set; }
        private float UnitTimeReservePerTurn = 5;


        private int currentTeamIndex;
        private int CurrentTeamIndex {
            get => currentTeamIndex;
            set => currentTeamIndex = value.ToRange(0, Rules.Teams.Count());
        }
        public Team CurrentTeam {
            get => Rules.Teams[currentTeamIndex];
            private set {
                var teamIndex = Rules.Teams.IndexOf(value);
                if (teamIndex == -1) {
                    throw new ArgumentException(nameof(value), "Заданная команда не найдена.");
                }

                currentTeamIndex = teamIndex;
            }
        }


        private ColoredChar[,] picture;
        public ColoredCharsPicture Picture { get; }



        public GameMap(LandMap landMap, Rules rules, IList<Unit> units) {
            LandMap = landMap;
            Rules = rules;
            Units = ExtractValidUnits(units).ToList();
            CurrentTeam = rules.Teams[0];
            picture = new ColoredChar[Height, Width];
            Picture = new ColoredCharsPicture(picture);
            HardRender();
        }



        public MaptileInfo this[int x, int y] {
            get {
                // REFACTORING: повторяющийся код.
                // Преобразовать в immutable класс с наследованием?
                if (UnitSelected) {
                    return new MaptileInfo(
                        LandMap[x, y],
                        new Point(x, y),
                        GetUnitOrNull(x, y),
                        picture[y, x],
                        MaptileReachableForSelectedUnit(new Point(x, y)),
                        MaptileLocationAvailableForSelectedUnitTempMove(new Point(x, y)),
                        MaptileIsSelectedUnitWay(new Point(x, y))
                    );
                }
                else {
                    return new MaptileInfo(
                        LandMap[x, y],
                        new Point(x, y),
                        null,
                        picture[y, x],
                        false,
                        false,
                        false
                    );
                }
            }
        }
        public MaptileInfo this[Point point] => this[point.X, point.Y];



        #region Unit
        public Unit GetUnitOrNull(Point location) => Units.FirstOrDefault((unit) => unit.Location == location);
        public Unit GetUnitOrNull(in int x, in int y) => GetUnitOrNull(new Point(x, y));


        public void SelectUnit() {
            Unit unit = GetUnitOrNull(SelectedTileLocation);
            if (unit is null || unit.Team != CurrentTeam) {
                return;
            }

            SelectedUnit = unit;
            UnitSelected = true;
            SelectedUnitTempRoute = new List<Point>();
            RefreshSelectedUnitAwailableRoutes();
        }
        // FEATURE: добавить разделение времени на выход/вход в тайл.

        public static float UnitTimeSpentOnTile(string landtileName, Unit unit) => speedPerTile / unit.CalculateSpeedOnLandtile(landtileName);


        public void AddSelectedUnitWay() {
            if (!UnitSelected
                || !MaptileLocationAvailableForSelectedUnitTempMove(SelectedTileLocation)
                || MaptileIsSelectedUnitWay(SelectedTileLocation)) {
                return;
            }

            string newPositionLandtileName = SelectedTile.Land.Name;
            SelectedUnitTempRoute.Add(SelectedTileLocation);
            RefreshSelectedUnitAwailableRoutes();
        }

        public bool MaptileLocationAvailableForSelectedUnitTempMove(Point landtileLocation) =>
            MaptileReachableForSelectedUnit(landtileLocation) &&
            TileClosestToSelectedUnitTempPosition(landtileLocation);

        public bool MaptileIsSelectedUnitWay(Point maptileLocation) => SelectedUnit.GetRoute().Contains(maptileLocation) || SelectedUnitTempRoute.Contains(maptileLocation);
        private bool TileClosestToSelectedUnitTempPosition(Point tile) {
            Point tempUnitPosition = GetSelectedUnitLastRoutePosition();
            return tempUnitPosition.CloseTo(tile);
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

        public bool MaptileReachableForSelectedUnit(Point tileLocation) => selectedUnitAvailableRoutes.Contains(tileLocation);

        public void ConfirmSelectedUnitRoute() {
            SelectedUnit.AddRoute(SelectedUnitTempRoute);
            UnselectUnit();
        }

        public void UnselectUnit() => UnitSelected = false;

        public void DeleteSelectedUnitLastWay() {
            if (!UnitSelected) { return; }

            bool wayRemoved = false;
            if (!SelectedUnitTempRoute.Empty()) {
                SelectedUnitTempRoute.RemoveLastItem();
                wayRemoved = true;
            }
            else
            if (SelectedUnit.RouteLength > 0) {
                if (SelectedUnit.RouteLength == 1) { SelectedUnit.TimeReserve = 0; }
                SelectedUnit.RemoveLastWay();
                wayRemoved = true;
            }

            if (wayRemoved) { RefreshSelectedUnitAwailableRoutes(); }
        }
        #endregion


        #region Turn
        public void ChangeTeam() {
            UnselectUnit();
            CurrentTeamIndex++;
        }
        public void MakeTurn() {
            MoveUnits(turnTimeTick);
            AttackUnits(turnTimeTick);
            RefreshTimers(turnTimeTick);
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

                    Landtile nextTile = LandMap[nextWayLocation.X, nextWayLocation.Y];
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
        private void AttackUnits(float allotedTime) {
            foreach (var unit in Units) {
                if (unit.WeaponCondition.CurrentCooldown > 0) {
                    unit.WeaponCondition.CurrentCooldown -= allotedTime;
                    continue;
                }

                List<Unit> targets = FindAwailableUnitTargets(unit);
                if (targets.Count == 0) { continue; }

                Unit target = targets.First();
                // REFACTORING: может, это инкапсулировать в unit?
                target.GetAttacked(unit.WeaponCondition.Weapon.Warhead);
                unit.WeaponCondition.CurrentCooldown = unit.WeaponCondition.Weapon.Cooldown;
            }
        }
        private List<Unit> FindAwailableUnitTargets(Unit unit) {
            // REFACTORING: Добавить дальность оружию.
            return Units.Except(new Unit[] { unit }).ToList();
        }
        private void RefreshTimers(float allotedTime) {
            foreach (var unit in Units) {
                unit.FlashTimer -= allotedTime;
            }
        }
        #endregion


        public void HardRender() {
            for (int r = 0; r < Height; r++) {
                for (int c = 0; c < Width; c++) {
                    picture[r, c] = LandMap[c, r].ColoredChar;
                }
            }

            foreach (var unit in Units) {
                picture[unit.Y, unit.X] = unit.ColoredChar;
            }

            if (UnitSelected) {
                ChangeColors(picture, selectedUnitAvailableRoutes, unitAvailableRoutesColor);
                ChangeColors(picture, SelectedUnit.GetRoute(), unitRouteColor);
                ChangeColors(picture, SelectedUnitTempRoute, unitRouteColor);
                picture[SelectedUnit.Location.Y, SelectedUnit.Location.X].Color = selectedUnitRoutingColor;
            }

            picture[SelectedTileY, SelectedTileX].Color = selectedTileColor;
            return;


            void ChangeColors(ColoredChar[,] CharPictures, IEnumerable<Point> coordList, ConsoleColor color) {
                foreach (var coord in coordList) {
                    CharPictures[coord.Y, coord.X].Color = color;
                }
            }
        }


        private IEnumerable<Unit> ExtractValidUnits(IEnumerable<Unit> units) => units.Distinct(new UnitLocationEqualsComparer());


        private List<Point> GetSelectedUnitAvailableRoutesPerTime(float timeReserve) {
            timeReserve = GetSelectedUnitRemainingTempTime(timeReserve);
            if (timeReserve <= 0) {
                return new List<Point>();
            }
            Point unitTempLocation = GetSelectedUnitLastRoutePosition();
            return FindUnitAvailableRoutesPerTime(unitTempLocation, SelectedUnit, timeReserve);
        }
        private float GetSelectedUnitRemainingTempTime(float timeReserve) {
            float remainingTime = SelectedUnit.TimeReserve + timeReserve;
            var route = new List<Point>(SelectedUnit.GetRoute());
            route.AddRange(SelectedUnitTempRoute);

            for (int i = 0; i < route.Count() && remainingTime > 0; i++) {
                Point way = route[i];
                Landtile land = LandMap[way];
                remainingTime -= UnitTimeSpentOnTile(land.Name, SelectedUnit);
            }
            return remainingTime;
        }
        private List<Point> FindUnitAvailableRoutesPerTime(Point unitStartLocation, Unit unit, float timeReserve) {
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
            bool tileExists = LandMap.TryGetLandtile(x, y, out Landtile landtile);
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
        private void RefreshSelectedUnitAwailableRoutes() {
            if (!UnitSelected) {
                throw new Exception();
            }
            selectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(UnitTimeReservePerTurn);

        }

    }
}
