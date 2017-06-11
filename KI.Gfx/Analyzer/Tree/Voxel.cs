using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Utility;

namespace KI.Gfx.Analyzer
{
    public class Voxel : IAnalyzer
    {
        /// <summary>
        /// ボクセルの状態
        /// </summary>
        public enum CellState
        {
            /// <summary>
            /// 未定
            /// </summary>
            None,
            /// <summary>
            /// 内側
            /// </summary>
            Inner,
            /// <summary>
            /// 外側
            /// </summary>
            Exterior,
            /// <summary>
            /// 境界
            /// </summary>
            Border,
        }

        public class Cell
        {
            public CellState State = CellState.None;
            public int i;
            public int j;
            public int k;
            public float Value;
            public Cell(int _i, int _j, int _k, CellState state)
            {
                i = _i;
                j = _j;
                k = _k;
                State = state;
                Value = 0;
            }
        }


        public Vector3 Min;
        public Vector3 Max;
        public float Interval;

        public int Partition
        {
            get;
            private set;
        }

        private Cell[, ,] Cells
        {
            get;
            set;
        }
        public List<Vector3> vPosition = new List<Vector3>();

        public List<Vector3> vNormal = new List<Vector3>();

        public Voxel(List<Vector3> position, List<int> posIndex, int partition)
        {

            Partition = partition;
            CalcMinMax(position);
            SetInterval(partition);
            Cells = new Cell[partition, partition, partition];
            Action<int, int, int> initializeCells = (int i, int j, int k) =>
              {
                  Cells[i, j, k] = new Cell(i, j, k, CellState.None);
              };

            AllCellAction(initializeCells);

            if (posIndex.Count == 0)
            {
                MakeVoxels(position);
            }
            else
            {
                List<Vector3> posStream = new List<Vector3>();
                for (int i = 0; i < posIndex.Count / 3; i++)
                {
                    posStream.Add(position[posIndex[3 * i]]);
                    posStream.Add(position[posIndex[3 * i + 1]]);
                    posStream.Add(position[posIndex[3 * i + 2]]);
                }
                MakeVoxels(posStream);
            }

            CalcDistanceField();
        }

        /// <summary>
        /// 形状を包括するBDBで、ボクセルを生成
        /// </summary>
        /// <param name="m_position"></param>
        /// <param name="posIndex"></param>
        private void MakeVoxels(List<Vector3> position)
        {
            //ボクセルのインデックス番号
            Vector3 vIndex = new Vector3();
            for (int i = 0; i < position.Count / 3; i++)
            {
                Vector3 tri1 = position[3 * i]; 
                Vector3 tri2 = position[3 * i + 1]; 
                Vector3 tri3 = position[3 * i + 2]; 

                //triを包括する部分のボクセルの最小値と最大値のインデックス
                Vector3 minIndex = MinVector(tri1, tri2);
                minIndex = MinVector(minIndex, tri3);

                Vector3 maxIndex = MaxVector(tri1, tri2);
                maxIndex = MaxVector(maxIndex, tri3);

                minIndex -= Min;
                maxIndex -= Min;
                minIndex /= Interval;
                maxIndex /= Interval;

                for (vIndex.X = (int)minIndex.X; vIndex.X < maxIndex.X; vIndex.X++)
                {
                    for (vIndex.Y = (int)minIndex.Y; vIndex.Y < maxIndex.Y; vIndex.Y++)
                    {
                        for (vIndex.Z = (int)minIndex.Z; vIndex.Z < maxIndex.Z; vIndex.Z++)
                        {
                            if (Cells[(int)vIndex.X, (int)vIndex.Y, (int)vIndex.Z].State == CellState.Border)
                            {
                                continue;
                            }

                            if (CheckVoxel(tri1, tri2, tri3, vIndex))
                            {
                                Cells[(int)vIndex.X, (int)vIndex.Y, (int)vIndex.Z].State = CellState.Border;
                                SetVoxel(vIndex);
                            }
                        }
                    }
                }
            }
        }

        public List<Vector3> GetPoint(CellState state)
        {
            List<Vector3> point = new List<Vector3>();
            var index = Vector3.Zero;
            var mid = Interval / 2;

            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        if (Cells[i, j, k].State == state)
                        {
                            index.X = i;
                            index.Y = j;
                            index.Z = k;
                            var position = GetVoxelPosition(index);
                            position.X += mid;
                            position.Y += mid;
                            position.Z += mid;
                            point.Add(position);
                        }
                    }
                }
            }
            return point;
        }

        public List<Cell> GetCell(CellState state)
        {
            List<Cell> cells = new List<Cell>();
            var index = Vector3.Zero;
            var mid = Interval / 2;

            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        if (Cells[i, j, k].State == state)
                        {
                            cells.Add(Cells[i, j, k]);
                        }
                    }
                }
            }
            return cells;
        }

        #region [utility]
        /// <summary>
        /// ボクセルの最小値を返却
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 GetCellPosition(Cell cell)
        {
            Vector3 minVoxel = Min;
            minVoxel.X += cell.i * Interval;
            minVoxel.Y += cell.j * Interval;
            minVoxel.Z += cell.k * Interval;
            return minVoxel;
        }

        /// <summary>
        /// ボクセルの最小値を返却
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 GetVoxelPosition(Vector3 index)
        {
            Vector3 minVoxel = Min;
            minVoxel += index * new Vector3(Interval);
            return minVoxel;
        }
        /// <summary>
        /// 分割数から長さを計算
        /// </summary>
        /// <param name="partition"></param>
        private void SetInterval(int partition)
        {
            float tmpmax, tmpmin;
            tmpmax = Max.X; tmpmin = Min.X;

            if (tmpmax - tmpmin < Max.Y - Min.Y) { tmpmax = Max.Y; tmpmin = Min.Y; }
            if (tmpmax - tmpmin < Max.Z - Min.Z) { tmpmax = Max.Z; tmpmin = Min.Z; }

            Interval = (tmpmax - tmpmin) / partition;
        }

        private void CalcMinMax(List<Vector3> position)
        {
            Min = position[0];
            Max = position[0];
            for (int i = 0; i < position.Count; i++)
            {
                if (Min.X > position[i].X) { Min.X = position[i].X; }
                if (Min.Y > position[i].Y) { Min.Y = position[i].Y; }
                if (Min.Z > position[i].Z) { Min.Z = position[i].Z; }

                if (Max.X < position[i].X) { Max.X = position[i].X; }
                if (Max.Y < position[i].Y) { Max.Y = position[i].Y; }
                if (Max.Z < position[i].Z) { Max.Z = position[i].Z; }

            }
        }
       

        private Vector3 MinVector(Vector3 v1, Vector3 v2)
        {
            Vector3 result = new Vector3(v1);
            if (result.X > v2.X) { result.X = v2.X; }
            if (result.Y > v2.Y) { result.Y = v2.Y; }
            if (result.Z > v2.Z) { result.Z = v2.Z; }

            return result;
        }
        private Vector3 MaxVector(Vector3 v1, Vector3 v2)
        {
            Vector3 result = new Vector3(v1);
            if (result.X < v2.X) { result.X = v2.X; }
            if (result.Y < v2.Y) { result.Y = v2.Y; }
            if (result.Z < v2.Z) { result.Z = v2.Z; }

            return result;
        }

        private bool InBox(Vector3 point,Vector3 min, Vector3 max)
        {
            if (min.X < point.X && point.X < max.X)
            {
                if (min.Y < point.Y && point.Y < max.Y)
                {
                    if (min.Z < point.Z && point.Z < max.Z)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// ボクセルを作るかチェック
        /// </summary>
        /// <param name="voxelIndex">voxelのindex番号</param>
        private bool CheckVoxel(Vector3 tri1, Vector3 tri2, Vector3 tri3, Vector3 voxelIndex)
        {
            Vector3 minVoxel = GetVoxelPosition(voxelIndex);
            Vector3 maxVoxel = minVoxel + new Vector3(Interval);

            //点によるチェック
            if (InBox(tri1, minVoxel, maxVoxel)) { return true; }
            if (InBox(tri2, minVoxel, maxVoxel)) { return true; }
            if (InBox(tri3, minVoxel, maxVoxel)) { return true; }


            //ボクセルの線と、三角形の面が交差するならtrue
            float maxValue = float.MaxValue;
            Vector3 tmp = new Vector3();
            //左下手前から反時計周り
            Vector3 v0 = minVoxel;
            Vector3 v1 = new Vector3(minVoxel.X + Interval, minVoxel.Y, minVoxel.Z);
            Vector3 v2 = new Vector3(minVoxel.X + Interval, minVoxel.Y + Interval, minVoxel.Z);
            Vector3 v3 = new Vector3(minVoxel.X, minVoxel.Y + Interval, minVoxel.Z);

            Vector3 v4 = new Vector3(minVoxel.X, minVoxel.Y, maxVoxel.Z + Interval);
            Vector3 v5 = new Vector3(minVoxel.X + Interval, minVoxel.Y, minVoxel.Z + Interval);
            Vector3 v6 = new Vector3(minVoxel.X + Interval, minVoxel.Y + Interval, minVoxel.Z + Interval);
            Vector3 v7 = new Vector3(minVoxel.X, minVoxel.Y + Interval, minVoxel.Z + Interval);

            //手前
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v0, v1, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v1, v2, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v2, v3, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v3, v1, ref maxValue, out tmp)) { return true; }
            //奥
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v4, v5, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v5, v6, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v6, v7, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v7, v4, ref maxValue, out tmp)) { return true; }
            //奥行き
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v0, v4, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v1, v5, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v2, v6, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(tri1, tri2, tri3, v3, v7, ref maxValue, out tmp)) { return true; }

            //三角形の線と、ボクセルの面が交差するならtrue
            //手前
            if (KICalc.CrossPlanetoLinePos(v0, v1, v2, v3, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v0, v1, v2, v3, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v0, v1, v2, v3, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //奥
            if (KICalc.CrossPlanetoLinePos(v4, v5, v6, v7, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v4, v5, v6, v7, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v4, v5, v6, v7, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //右
            if (KICalc.CrossPlanetoLinePos(v1, v5, v6, v2, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v1, v5, v6, v2, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v1, v5, v6, v2, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //左
            if (KICalc.CrossPlanetoLinePos(v0, v3, v7, v4, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v0, v3, v7, v4, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v0, v3, v7, v4, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //上
            if (KICalc.CrossPlanetoLinePos(v2, v6, v7, v3, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v2, v6, v7, v3, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v2, v6, v7, v3, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //下
            if (KICalc.CrossPlanetoLinePos(v0, v4, v5, v1, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v0, v4, v5, v1, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (KICalc.CrossPlanetoLinePos(v0, v4, v5, v1, tri3, tri1, ref maxValue, out tmp)) { return true; }
            
            return false;
        }

        private Cell SerchNeightCell(Cell cell, Func<int, int, int, bool> condition)
        {
            if (condition(cell.i + 1, cell.j, cell.k))
            {
                return Cells[cell.i + 1, cell.j, cell.k];
            }
            if (condition(cell.i, cell.j + 1, cell.k))
            {
                return Cells[cell.i, cell.j + 1, cell.k];
            }
            if (condition(cell.i, cell.j, cell.k + 1))
            {
                return Cells[cell.i, cell.j, cell.k + 1];
            }
            if (condition(cell.i - 1, cell.j, cell.k))
            {
                return Cells[cell.i - 1, cell.j, cell.k];
            }
            if (condition(cell.i, cell.j - 1, cell.k))
            {
                return Cells[cell.i, cell.j - 1, cell.k];
            }
            if (condition(cell.i, cell.j, cell.k - 1))
            {
                return Cells[cell.i, cell.j, cell.k - 1];
            }
            return null;
        }

        private Cell SearchAllVoxel(Func<int, int, int, Cell> condition)
        {
            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        Cell value = condition(i, j, k);
                        if(value != null)
                        {
                            return value;
                        }
                    }
                }
            }
            return null;
        }

        private void AllCellAction(Action<int, int, int> action)
        {
            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        action(i, j, k);
                    }
                }
            }
        }

        private Cell StartDistanceFieldCell()
        {
            Func<int, int, int, bool> checkInner = (i, j, k) =>
            {
                var cell = GetCell(i, j, k);
                if (cell?.State == CellState.Inner &&
                cell?.Value == float.MaxValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };

            Func<int, int, int, Cell> neightInner = (int i, int j, int k) =>
             {
                 if (Cells[i, j, k].State == CellState.Border)
                 {
                     Cell hit = SerchNeightCell(Cells[i, j, k], checkInner);
                     if (hit != null)
                     {
                         return hit;
                     }
                 }
                 return null;
             };

            return SearchAllVoxel(neightInner);
        }
        /// <summary>
        /// 距離場の算出
        /// </summary>
        private void CalcDistanceField()
        {
            CalcInOut();
            Action<int, int, int> initializeField = (int i, int j, int k) =>
            {
                if (Cells[i, j, k].State == CellState.Inner)
                {
                    Cells[i, j, k].Value = float.MaxValue;
                }
                else
                {
                    Cells[i, j, k].Value = 0;
                }
            };
            AllCellAction(initializeField);

            while (true)
            {
                Cell start = StartDistanceFieldCell();
                if (start == null)
                {
                    return;
                }

                Func<Cell, bool> condition = (cell) =>
                {
                    if (cell.State == CellState.Inner &&
                        cell.Value == float.MaxValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };

                Func<int, int, int, Cell> minValue = (i, j, k) =>
                {
                    return null;
                };
                Action<Cell> action = (cell) =>
                {
                    float min = float.MaxValue;
                    Cell minCell = null;
                    var neight = GetCell(cell.i + 1, cell.j, cell.k);
                    if (neight?.Value < min)
                    {
                        min = neight.Value;
                        minCell = neight;
                    }
                    neight = GetCell(cell.i, cell.j + 1, cell.k);
                    if (neight?.Value < min)
                    {
                        min = neight.Value;
                        minCell = neight;
                    }
                    neight = GetCell(cell.i, cell.j, cell.k + 1);
                    if (neight?.Value < min)
                    {
                        min = neight.Value;
                        minCell = neight;
                    }
                    neight = GetCell(cell.i - 1, cell.j, cell.k);
                    if (neight?.Value < min)
                    {
                        min = neight.Value;
                        minCell = neight;
                    }
                    neight = GetCell(cell.i, cell.j - 1, cell.k);
                    if (neight?.Value < min)
                    {
                        min = neight.Value;
                        minCell = neight;
                    }
                    neight = GetCell(cell.i, cell.j, cell.k - 1);
                    if (neight?.Value < min)
                    {
                        min = neight.Value;
                        minCell = neight;
                    }

                    if (minCell != null)
                    {
                        cell.Value = (GetCellPosition(cell) - GetCellPosition(minCell)).Length + minCell.Value;

                        if (cell.Value > maxDistance)
                        {
                            maxDistance = cell.Value;
                        }
                    }
                };

                RegionGrowing(start, condition, action);
            }
        }
        public static float maxDistance = 0;

        private void RegionGrowing(Cell seed, Func<Cell, bool> condition, Action<Cell> action)
        {
            var stack = new Stack<Cell>();
            Cell curerntCell = null;
            Cell cell = null;
            if (condition(seed))
            {
                action(seed);
                curerntCell = seed;
            }
            else
            {
                return;
            }

            
            while (true)
            {
                cell = GetCell(curerntCell.i + 1, curerntCell.j, curerntCell.k);
                if (condition(cell))
                {
                    action(cell);
                    stack.Push(cell);
                    curerntCell = cell;
                    continue;
                }
                else
                {
                    cell = GetCell(curerntCell.i, curerntCell.j + 1, curerntCell.k);
                    if (condition(cell))
                    {
                        action(cell);
                        stack.Push(cell);
                        curerntCell = cell;
                        continue;
                    }
                    else
                    {
                        cell = GetCell(curerntCell.i, curerntCell.j, curerntCell.k + 1);
                        if (condition(cell))
                        {
                            action(cell);
                            stack.Push(cell);
                            curerntCell = cell;
                            continue;
                        }
                        else
                        {
                            cell = GetCell(curerntCell.i - 1, curerntCell.j, curerntCell.k);
                            if (condition(cell))
                            {
                                action(cell);
                                stack.Push(cell);
                                curerntCell = cell;
                                continue;
                            }
                            else
                            {
                                cell = GetCell(curerntCell.i, curerntCell.j - 1, curerntCell.k);
                                if (condition(cell))
                                {
                                    action(cell);
                                    stack.Push(cell);
                                    curerntCell = cell;
                                    continue;
                                }
                                else
                                {
                                    cell = GetCell(curerntCell.i, curerntCell.j, curerntCell.k - 1);
                                    if (condition(cell))
                                    {
                                        action(cell);
                                        stack.Push(cell);
                                        curerntCell = cell;
                                        continue;
                                    }
                                    else
                                    {
                                        if (stack.Count > 0)
                                        {
                                            curerntCell = stack.Pop();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ボクセルの内外判定
        /// </summary>
        private void CalcInOut()
        {
            Func<Cell, bool> condition = (cell) =>
                 {
                     return cell?.State == CellState.None;
                 };
            Action<Cell> action = (cell) =>
            {
                cell.State = CellState.Exterior;
            };

            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    var max = Partition - 1;
                    RegionGrowing(Cells[0, i, j], condition, action);
                    RegionGrowing(Cells[i, 0, j], condition, action);
                    RegionGrowing(Cells[i, j, 0], condition, action);
                    RegionGrowing(Cells[max, i, j], condition, action);
                    RegionGrowing(Cells[i, max, j], condition, action);
                    RegionGrowing(Cells[i, j, max], condition, action);
                }
            }

            Action<int, int, int> setInner = (int i, int j, int k) =>
              {
                  if (Cells[i, j, k].State == CellState.None)
                  {
                      Cells[i, j, k].State = CellState.Inner;
                  }
              };

            AllCellAction(setInner);
        }

        private Cell GetCell(int i, int j, int k)
        {
            if (i < 0 || j < 0 || k < 0 ||
                i >= Partition ||
                j >= Partition ||
                k >= Partition)
            {
                return null;
            }
            else
            {
                return Cells[i, j, k];
            }
        }

        /// <summary>
        /// Region Growing Loopに置き換え
        /// </summary>
        /// <param name="seed"></param>
        //private void CheckOutStack(Cell seed)
        //{
        //    var stack = new Stack<Cell>();
        //    if (seed?.State == CellState.None)
        //    {
        //        seed.State = CellState.Exterior;
        //        stack.Push(seed);
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    Cell curerntCell = stack.Pop();
        //    Cell cell = null;
        //    while (true)
        //    {
        //        cell = GetCell(curerntCell.i + 1, curerntCell.j, curerntCell.k);
        //        if (cell?.State == CellState.None)
        //        {
        //            cell.State = CellState.Exterior;
        //            stack.Push(cell);
        //            curerntCell = cell;
        //            continue;
        //        }
        //        else
        //        {
        //            cell = GetCell(curerntCell.i, curerntCell.j + 1, curerntCell.k);
        //            if (cell?.State == CellState.None)
        //            {
        //                cell.State = CellState.Exterior;
        //                stack.Push(cell);
        //                curerntCell = cell;
        //                continue;
        //            }
        //            else
        //            {
        //                cell = GetCell(curerntCell.i, curerntCell.j, curerntCell.k + 1);
        //                if (cell?.State == CellState.None)
        //                {
        //                    cell.State = CellState.Exterior;
        //                    stack.Push(cell);
        //                    curerntCell = cell;
        //                    continue;
        //                }
        //                else
        //                {
        //                    cell = GetCell(curerntCell.i - 1, curerntCell.j, curerntCell.k);
        //                    if (cell?.State == CellState.None)
        //                    {
        //                        cell.State = CellState.Exterior;
        //                        stack.Push(cell);
        //                        curerntCell = cell;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        cell = GetCell(curerntCell.i, curerntCell.j - 1, curerntCell.k);
        //                        if (cell?.State == CellState.None)
        //                        {
        //                            cell.State = CellState.Exterior;
        //                            stack.Push(cell);
        //                            curerntCell = cell;
        //                            continue;
        //                        }
        //                        else
        //                        {
        //                            cell = GetCell(curerntCell.i, curerntCell.j, curerntCell.k - 1);
        //                            if (cell?.State == CellState.None)
        //                            {
        //                                cell.State = CellState.Exterior;
        //                                stack.Push(cell);
        //                                curerntCell = cell;
        //                                continue;
        //                            }
        //                            else
        //                            {
        //                                if (stack.Count > 0)
        //                                {
        //                                    curerntCell = stack.Pop();
        //                                }
        //                                else
        //                                {
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        #region [voxel object]
        /// <summary>
        /// ボクセルをvoxelPositionにセットする
        /// </summary>
        private void SetVoxel(Vector3 voxelIndex)
        {
            Vector3 minVoxel = GetVoxelPosition(voxelIndex);
            Vector3 maxVoxel = minVoxel + new Vector3(Interval);
            //minVoxel += new Vector3(m_Length * 0.1f);
            //maxVoxel -= new Vector3(m_Length * 0.1f);
            Vector3 v0 = new Vector3(minVoxel.X, minVoxel.Y, minVoxel.Z);
            Vector3 v1 = new Vector3(maxVoxel.X, minVoxel.Y, minVoxel.Z);
            Vector3 v2 = new Vector3(maxVoxel.X, maxVoxel.Y, minVoxel.Z);
            Vector3 v3 = new Vector3(minVoxel.X, maxVoxel.Y, minVoxel.Z);
            Vector3 v4 = new Vector3(minVoxel.X, minVoxel.Y, maxVoxel.Z);
            Vector3 v5 = new Vector3(maxVoxel.X, minVoxel.Y, maxVoxel.Z);
            Vector3 v6 = new Vector3(maxVoxel.X, maxVoxel.Y, maxVoxel.Z);
            Vector3 v7 = new Vector3(minVoxel.X, maxVoxel.Y, maxVoxel.Z);
            //手前
            SetQuad(v0, v3, v2, v1);
            //右
            SetQuad(v1, v2, v6, v5);
            //左
            SetQuad(v0, v4, v7, v3);
            //奥
            SetQuad(v4, v5, v6, v7);
            //上
            SetQuad(v2, v3, v7, v6);
            //下
            SetQuad(v1, v5, v4, v0);
        }

        private void SetQuad(Vector3 q0, Vector3 q1, Vector3 q2, Vector3 q3)
        {
            Vector3 normal;
            vPosition.Add(q0); vPosition.Add(q1); vPosition.Add(q2); vPosition.Add(q3);
            normal = KICalc.Normal(q1 - q0, q2 - q0);
            vNormal.Add(normal);
            vNormal.Add(normal);
            vNormal.Add(normal);
            vNormal.Add(normal);
        }
        
        #endregion

        public void GetVoxel(out List<Vector3> position,out List<Vector3> normal)
        {
            position = vPosition;
            normal = vNormal;
        }
    }
}
