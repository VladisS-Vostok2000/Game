﻿using System;
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


        public int LengthX => LandtilesMap.GetUpperBound(0) + 1;
        public int LengthY => LandtilesMap.GetUpperBound(1) + 1;
        public int Square => LengthX * LengthY;
        // REFACTORING: заменить на лист?
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
        public MaptileInfo SelectedTile => this[SelectedTileX, SelectedTileY];


        private const ConsoleColor unitAvailableRoutesColor = ConsoleColor.Cyan;
        private const ConsoleColor unitRouteColor = ConsoleColor.Red;
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
        public ICollection<Point> SelectedUnitAvailableRoutes { get; private set; }
        private IList<Point> SelectedUnitRouteTemp;
        private float SelectedUnitTimeReserveTemp;



        public MaptileInfo this[int x, int y] {
            get {
                if (UnitSelected) {
                    return new MaptileInfo(
                        LandtilesMap[x, y],
                        Units[x, y],
                        MaptileReachableForSelectedUnit(new Point(x, y)),
                        MaptileLocationAvailableForSelectedUnitMove(new Point(x, y)),
                        MaptileIsSelectedUnitRoute(new Point(x, y))
                    );
                }
                else {
                    return new MaptileInfo(
                        LandtilesMap[x, y],
                        Units[x, y],
                        false,
                        false,
                        false
                    );
                }
            }
        }
        public MaptileInfo this[Point point] => this[point.X, point.Y];



        public Map(Landtile[,] landtiles, Rules rules, Unit[,] units) {
            LandtilesMap = landtiles;
            Rules = rules;
            Units = units;
        }





        public ConsoleImage[,] Visualize() {
            var outArray = new ConsoleImage[LengthX, LengthY];

            for (int r = 0; r < LengthY; r++) {
                for (int c = 0; c < LengthX; c++) {
                    outArray[c, r] = this[c, r].ToConsoleImage();
                }
            }

            // REFACTORING: можно выделить функции ниже в локальные.
            if (UnitSelected) {
                ChangeColors(outArray, SelectedUnitAvailableRoutes, unitAvailableRoutesColor);
                ChangeColors(outArray, SelectedUnit.Route, unitRouteColor);
                ChangeColors(outArray, SelectedUnitRouteTemp, unitRouteColor);
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


        public void SelectUnit() {
            Unit unit = Units[SelectedTileX, SelectedTileY];
            if (unit == null) {
                return;
            }

            SelectedUnit = unit;
            SelectedUnitLocation = SelectedTileLocation;
            // WORKAROUND: пока не реализован ход, запускаю так
            if (SelectedUnit.Route.Count == 0) {
                SelectedUnit.Route.Add(SelectedUnitLocation);
            }
            SelectedUnitRouteTemp = new List<Point>();
            SelectedUnitRouteTemp.AddRange(SelectedUnit.Route);
            SelectedUnitTimeReserveTemp = SelectedUnit.TimeReserve;
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(SelectedUnitTimeReserveTemp);
            UnitSelected = true;
        }
        // FEATURE: добавить разделение времени на выход/вход в тайл.
        private List<Point> GetSelectedUnitAvailableRoutesPerTime(float timeReserve) {
            var availableRoutes = new List<Point>();
            Point startPoint = !SelectedUnitRouteTemp.Empty() ? SelectedUnitRouteTemp.Last() : SelectedUnit.Route.Last();
            // REFACTORING: подключить многопоточность?
            var tempList = new List<Point>();
            tempList.AddRange(FindUnitAvailableRoutesPerTime(startPoint.X + 1, startPoint.Y, SelectedUnit, timeReserve));
            tempList.AddRange(FindUnitAvailableRoutesPerTime(startPoint.X - 1, startPoint.Y, SelectedUnit, timeReserve));
            tempList.AddRange(FindUnitAvailableRoutesPerTime(startPoint.X, startPoint.Y + 1, SelectedUnit, timeReserve));
            tempList.AddRange(FindUnitAvailableRoutesPerTime(startPoint.X, startPoint.Y - 1, SelectedUnit, timeReserve));
            // REFACTORING: такое себе решение.
            var outList = new List<Point>();
            foreach (var location in tempList) {
                if (!outList.Contains(location)) {
                    outList.Add(location);
                }
            }
            return outList;
        }
        // REFACTORING: добавить IMoveable: иначе unit непонятно что здесь делает.
        private List<Point> FindUnitAvailableRoutesPerTime(in int x, in int y, in Unit unit, double unitTimeReserve) {
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
            // REFACTORING: такое себе решение.
            if (!outList.Contains(new Point(x, y))) {
                outList.Add(new Point(x, y));
            }
            unitTimeReserve -= timeSpent;
            // REFACTORING: подключить многопоточность?
            outList.AddRange(FindUnitAvailableRoutesPerTime(x + 1, y, unit, unitTimeReserve));
            outList.AddRange(FindUnitAvailableRoutesPerTime(x - 1, y, unit, unitTimeReserve));
            outList.AddRange(FindUnitAvailableRoutesPerTime(x, y + 1, unit, unitTimeReserve));
            outList.AddRange(FindUnitAvailableRoutesPerTime(x, y - 1, unit, unitTimeReserve));
            return outList;
        }
        private bool TryGetTile(int landtileX, int landtileY, out MaptileInfo landtile) {
            bool correctIndexation = landtileX.IsInRange(0, LengthX - 1) && landtileY.IsInRange(0, LengthY - 1);
            landtile = correctIndexation ? this[landtileX, landtileY] : default;
            return correctIndexation;
        }


        public static float UnitTimeSpentOnTile(string landtileName, Unit unit) => speedPerTile / unit.CalculateSpeed(landtileName);


        public void AddSelectedUnitRoute() {
            if (!UnitSelected
                || !MaptileLocationAvailableForSelectedUnitMove(SelectedTileLocation)
                || MaptileIsSelectedUnitRoute(SelectedTileLocation)) {
                return;
            }

            string newPositionLandtileName = SelectedTile.Land.Name;
            float timeSpent = UnitTimeSpentOnTile(newPositionLandtileName, SelectedUnit);
            SelectedUnitTimeReserveTemp -= timeSpent;
            SelectedUnitRouteTemp.Add(SelectedTileLocation);
            SelectedUnitAvailableRoutes = GetSelectedUnitAvailableRoutesPerTime(SelectedUnitTimeReserveTemp);
        }
        public bool MaptileLocationAvailableForSelectedUnitMove(Point landtileLocation) =>
            MaptileReachableForSelectedUnit(landtileLocation) &&
            TileClosestToSelectedUnitPosition(landtileLocation);
        public bool MaptileIsSelectedUnitRoute(Point maptileLocation) => SelectedUnit.Route.Contains(maptileLocation) || SelectedUnitRouteTemp.Contains(maptileLocation);
        public bool TileClosestToSelectedUnitPosition(Point tile) {
            Point unitPosition = SelectedUnitRouteTemp.Count == 0 ? SelectedUnit.Route.Last() : SelectedUnitRouteTemp.Last();
            return (Math.Abs(tile.X - unitPosition.X) == 1 && tile.Y == unitPosition.Y)
                || (Math.Abs(tile.Y - unitPosition.Y) == 1 && tile.X == unitPosition.X);
        }
        public bool MaptileReachableForSelectedUnit(Point tileLocation) => SelectedUnitAvailableRoutes.Contains(tileLocation);
        public void ConfirmSelectedUnitRoute() {
            SelectedUnit.Route.AddRange(SelectedUnitRouteTemp);
            SelectedUnit.TimeReserve = SelectedUnitTimeReserveTemp;
            UnselectUnit();
        }
        public void UnselectUnit() => UnitSelected = false;

    }
}
