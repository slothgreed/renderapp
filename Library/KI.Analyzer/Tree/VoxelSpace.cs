using System;
using System.Collections.Generic;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ボクセルの状態
    /// </summary>
    public enum VoxelState
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

    /// <summary>
    /// 近傍種類
    /// </summary>
    public enum NeightType
    {
        /// <summary>
        /// 6近傍
        /// </summary>
        Ortho,

        /// <summary>
        /// 全て
        /// </summary>
        All
    }

    /// <summary>
    /// 頂点位置小文字は最小値大文字は最大値のほう
    /// </summary>
    public enum CubeVertex
    {
        xyZ = 0,
        XyZ,
        Xyz,
        xyz,
        xYZ,
        XYZ,
        XYz,
        xYz,
        Num
    }

    /// <summary>
    /// ボクセル空間
    /// </summary>
    public class VoxelSpace
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">位置座標</param>
        /// <param name="posIndex">インデックス</param>
        /// <param name="partition">分割数</param>
        /// <param name="min">空間の最小値</param>
        /// <param name="max">空間の最大値</param>
        public VoxelSpace(List<Vector3> position, List<int> posIndex, int partition, Vector3 min, Vector3 max)
        {
            Partition = partition;
            SpaceMin = min;
            SpaceMax = max;
            SetInterval();
            Voxels = new Voxel[Partition, Partition, Partition];

            Action<int, int, int> initializeVoxels = (int i, int j, int k) =>
              {
                  Voxels[i, j, k] = new Voxel(i, j, k, VoxelState.None);
              };

            VoxelAction(initializeVoxels);

            if (posIndex.Count == 0)
            {
                CalculateBorder(position);
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

                CalculateBorder(posStream);
            }

            CalculateInOut();
        }

        /// <summary>
        /// 最小値
        /// </summary>
        public Vector3 SpaceMin { get; private set; }

        /// <summary>
        /// 最大値
        /// </summary>
        public Vector3 SpaceMax { get; private set; }

        /// <summary>
        /// ボクセル
        /// </summary>
        public Voxel[,,] Voxels { get; private set; }

        /// <summary>
        /// 間隔
        /// </summary>
        public float Interval { get; private set; }

        /// <summary>
        /// 分割数
        /// </summary>
        public int Partition { get; private set; }

        /// <summary>
        /// ボクセルの取得
        /// </summary>
        /// <param name="xIndex">x要素番号</param>
        /// <param name="yIndex">y要素番号</param>
        /// <param name="zIndex">z要素番号</param>
        /// <returns>ボクセル</returns>
        public Voxel GetVoxel(int xIndex, int yIndex, int zIndex)
        {
            if (xIndex < 0 || yIndex < 0 || zIndex < 0 ||
                xIndex >= Partition ||
                yIndex >= Partition ||
                zIndex >= Partition)
            {
                return null;
            }
            else
            {
                return Voxels[xIndex, yIndex, zIndex];
            }
        }

        /// <summary>
        /// 指定した状態のボクセルを取得します。
        /// </summary>
        /// <param name="state">状態</param>
        /// <returns>ボクセル</returns>
        public List<Voxel> GetVoxel(VoxelState state)
        {
            List<Voxel> voxels = new List<Voxel>();
            var index = Vector3.Zero;
            var mid = Interval / 2;

            Action<int, int, int> addList = (int i, int j, int k) =>
            {
                if (this.Voxels[i, j, k].State == state)
                {
                    voxels.Add(this.Voxels[i, j, k]);
                }
            };

            VoxelAction(addList);

            return voxels;
        }

        /// <summary>
        /// 指定した状態の頂点を取得します。
        /// </summary>
        /// <param name="state">状態</param>
        /// <returns>ボクセル</returns>
        public List<Vector3> GetPoint(VoxelState state)
        {
            List<Vector3> point = new List<Vector3>();
            var mid = Interval / 2;

            Action<int, int, int> addPoint = (int i, int j, int k) =>
            {
                if (Voxels[i, j, k].State == state)
                {
                    var position = GetVoxelPosition(i, j, k);
                    position.X += mid;
                    position.Y += mid;
                    position.Z += mid;
                    point.Add(position);
                }
            };

            VoxelAction(addPoint);

            return point;
        }

        /// <summary>
        /// ボクセルの最小座標を返却
        /// </summary>
        /// <param name="voxel">ボクセル</param>
        /// <returns>座標</returns>
        public Vector3 GetVoxelPosition(Voxel voxel)
        {
            return GetVoxelPosition(voxel.X, voxel.Y, voxel.Z);
        }

        /// <summary>
        /// ボクセルの最小値を返却
        /// </summary>
        /// <param name="xIndex">x要素番号</param>
        /// <param name="yIndex">y要素番号</param>
        /// <param name="zIndex">z要素番号</param>
        /// <returns></returns>
        public Vector3 GetPosition(int xIndex, int yIndex, int zIndex, CubeVertex state)
        {
            Vector3 position = SpaceMin;

            position.X += xIndex * Interval;
            position.Y += yIndex * Interval;
            position.Z += zIndex * Interval;

            switch (state)
            {
                case CubeVertex.xyZ:
                    position.Z += Interval;
                    break;
                case CubeVertex.XyZ:
                    position.X += Interval;
                    position.Z += Interval;
                    break;
                case CubeVertex.Xyz:
                    position.X += Interval;
                    break;
                case CubeVertex.xyz:
                    break;
                case CubeVertex.xYZ:
                    position.Y += Interval;
                    position.Z += Interval;
                    break;
                case CubeVertex.XYZ:
                    position.X += Interval;
                    position.Y += Interval;
                    position.Z += Interval;
                    break;
                case CubeVertex.XYz:
                    position.X += Interval;
                    position.Y += Interval;
                    break;
                case CubeVertex.xYz:
                    position.Y += Interval;
                    break;
                default:
                    break;
            }

            return position;
        }

        /// <summary>
        /// ボクセルの最小座標を返却
        /// </summary>
        /// <param name="xIndex">x要素番号</param>
        /// <param name="yIndex">y要素番号</param>
        /// <param name="zIndex">z要素番号</param>
        /// <returns>座標</returns>
        public Vector3 GetVoxelPosition(int xIndex, int yIndex, int zIndex)
        {
            Vector3 minVoxel = SpaceMin;
            minVoxel.X += xIndex * Interval;
            minVoxel.Y += yIndex * Interval;
            minVoxel.Z += zIndex * Interval;
            return minVoxel;
        }

        #region [action]
        /// <summary>
        /// 隣接ボクセルの条件に合うボクセルを取得する。
        /// </summary>
        /// <param name="voxel">ボクセル</param>
        /// <param name="condition">条件</param>
        /// <param name="type">隣接範囲</param>
        /// <returns>条件に合った隣接ボクセル</returns>
        public IEnumerable<Voxel> SearchNeightVoxel(Voxel voxel, Func<int, int, int, bool> condition, NeightType type)
        {
            var search = new List<Voxel>();

            if (type == NeightType.Ortho)
            {
                if (condition(voxel.X + 1, voxel.Y, voxel.Z))
                    search.Add(GetVoxel(voxel.X + 1, voxel.Y, voxel.Z));
                if (condition(voxel.X, voxel.Y + 1, voxel.Z))
                    search.Add(GetVoxel(voxel.X, voxel.Y + 1, voxel.Z));
                if (condition(voxel.X, voxel.Y, voxel.Z + 1))
                    search.Add(GetVoxel(voxel.X, voxel.Y, voxel.Z + 1));
                if (condition(voxel.X - 1, voxel.Y, voxel.Z))
                    search.Add(GetVoxel(voxel.X - 1, voxel.Y, voxel.Z));
                if (condition(voxel.X, voxel.Y - 1, voxel.Z))
                    search.Add(GetVoxel(voxel.X, voxel.Y - 1, voxel.Z));
                if (condition(voxel.X, voxel.Y, voxel.Z - 1))
                    search.Add(GetVoxel(voxel.X, voxel.Y, voxel.Z - 1));
            }
            else if (type == NeightType.All)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            if (x == 0 && y == 0 && z == 0)
                            {
                                continue;
                            }

                            if (condition(voxel.X + x, voxel.Y + y, voxel.Z + z))
                            {
                                search.Add(GetVoxel(voxel.X + x, voxel.Y + y, voxel.Z + z));
                            }
                        }
                    }
                }
            }

            return search;
        }

        /// <summary>
        /// すべてのボクセルから、条件にあったボクセルを検索
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>最初に成功したボクセル</returns>
        public Voxel SearchAllVoxel(Func<int, int, int, Voxel> condition)
        {
            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        Voxel value = condition(i, j, k);
                        if (value != null)
                        {
                            return value;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 条件に合ったボクセルに合う処理
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="action"></param>
        public void VoxelAction(Func<int, int, int, bool> condition, Action<int, int, int> action)
        {
            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        if (condition(i, j, k))
                        {
                            continue;
                        }

                        action(i, j, k);
                    }
                }
            }
        }

        /// <summary>
        /// 全てのボクセルに対する処理
        /// </summary>
        /// <param name="action">処理</param>
        public void VoxelAction(Action<int, int, int> action)
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

        /// <summary>
        /// 領域拡張
        /// </summary>
        /// <param name="seed">一つ目のセル</param>
        /// <param name="condition">条件</param>
        /// <param name="action">処理</param>
        public void RegionGrowing(Voxel seed, Func<Voxel, bool> condition, Action<Voxel> action)
        {
            var stack = new Stack<Voxel>();
            Voxel curerntVoxel = null;
            Voxel voxel = null;
            if (condition(seed))
            {
                action(seed);
                curerntVoxel = seed;
            }
            else
            {
                return;
            }

            while (true)
            {
                voxel = GetVoxel(curerntVoxel.X + 1, curerntVoxel.Y, curerntVoxel.Z);
                if (condition(voxel))
                {
                    action(voxel);
                    stack.Push(voxel);
                    curerntVoxel = voxel;
                    continue;
                }
                else
                {
                    voxel = GetVoxel(curerntVoxel.X, curerntVoxel.Y + 1, curerntVoxel.Z);
                    if (condition(voxel))
                    {
                        action(voxel);
                        stack.Push(voxel);
                        curerntVoxel = voxel;
                        continue;
                    }
                    else
                    {
                        voxel = GetVoxel(curerntVoxel.X, curerntVoxel.Y, curerntVoxel.Z + 1);
                        if (condition(voxel))
                        {
                            action(voxel);
                            stack.Push(voxel);
                            curerntVoxel = voxel;
                            continue;
                        }
                        else
                        {
                            voxel = GetVoxel(curerntVoxel.X - 1, curerntVoxel.Y, curerntVoxel.Z);
                            if (condition(voxel))
                            {
                                action(voxel);
                                stack.Push(voxel);
                                curerntVoxel = voxel;
                                continue;
                            }
                            else
                            {
                                voxel = GetVoxel(curerntVoxel.X, curerntVoxel.Y - 1, curerntVoxel.Z);
                                if (condition(voxel))
                                {
                                    action(voxel);
                                    stack.Push(voxel);
                                    curerntVoxel = voxel;
                                    continue;
                                }
                                else
                                {
                                    voxel = GetVoxel(curerntVoxel.X, curerntVoxel.Y, curerntVoxel.Z - 1);
                                    if (condition(voxel))
                                    {
                                        action(voxel);
                                        stack.Push(voxel);
                                        curerntVoxel = voxel;
                                        continue;
                                    }
                                    else
                                    {
                                        if (stack.Count > 0)
                                        {
                                            curerntVoxel = stack.Pop();
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

        #endregion

        /// <summary>
        /// 分割数から長さを計算
        /// </summary>
        private void SetInterval()
        {
            float tmpmax, tmpmin;
            tmpmax = SpaceMax.X; tmpmin = SpaceMin.X;

            if (tmpmax - tmpmin < SpaceMax.Y - SpaceMin.Y) { tmpmax = SpaceMax.Y; tmpmin = SpaceMin.Y; }
            if (tmpmax - tmpmin < SpaceMax.Z - SpaceMin.Z) { tmpmax = SpaceMax.Z; tmpmin = SpaceMin.Z; }

            Interval = (tmpmax - tmpmin) / Partition;
        }

        #region [calculate voxel info]
        /// <summary>
        /// 形状を包括するBDBで、ボクセルの境界判定
        /// </summary>
        /// <param name="position">頂点リスト</param>
        private void CalculateBorder(List<Vector3> position)
        {
            //ボクセルのインデックス番号
            for (int i = 0; i < position.Count / 3; i++)
            {
                Vector3 tri1 = position[3 * i];
                Vector3 tri2 = position[3 * i + 1];
                Vector3 tri3 = position[3 * i + 2];

                //triを包括する部分のボクセルの最小値と最大値のインデックス
                Vector3 minIndex = Vector3.ComponentMin(tri1, tri2);
                minIndex = Vector3.ComponentMin(minIndex, tri3);

                Vector3 maxIndex = Vector3.ComponentMax(tri1, tri2);
                maxIndex = Vector3.ComponentMax(maxIndex, tri3);

                minIndex -= SpaceMin;
                maxIndex -= SpaceMin;
                minIndex /= Interval;
                maxIndex /= Interval;

                for (int xIndex = (int)minIndex.X; xIndex < maxIndex.X; xIndex++)
                {
                    for (int yIndex = (int)minIndex.Y; yIndex < maxIndex.Y; yIndex++)
                    {
                        for (int zIndex = (int)minIndex.Z; zIndex < maxIndex.Z; zIndex++)
                        {
                            if (Voxels[xIndex, yIndex, zIndex].State == VoxelState.Border)
                            {
                                continue;
                            }

                            if (CheckVoxel(tri1, tri2, tri3, xIndex, yIndex, zIndex))
                            {
                                Voxels[xIndex, yIndex, zIndex].State = VoxelState.Border;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ボクセルを作るかチェック
        /// </summary>
        /// <param name="tri1">三角形頂点1</param>
        /// <param name="tri2">三角形頂点2</param>
        /// <param name="tri3">三角形頂点3</param>
        /// <param name="xIndex">x要素番号</param>
        /// <param name="yIndex">y要素番号</param>
        /// <param name="zIndex">z要素番号</param>
        /// <returns>作る</returns>
        private bool CheckVoxel(Vector3 tri1, Vector3 tri2, Vector3 tri3, int xIndex, int yIndex, int zIndex)
        {
            Vector3 minVoxel = GetVoxelPosition(xIndex, yIndex, zIndex);
            Vector3 maxVoxel = minVoxel + new Vector3(Interval);

            //点によるチェック
            if (Inside.Box(tri1, minVoxel, maxVoxel)) { return true; }
            if (Inside.Box(tri2, minVoxel, maxVoxel)) { return true; }
            if (Inside.Box(tri3, minVoxel, maxVoxel)) { return true; }

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
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v0, v1, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v1, v2, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v2, v3, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v3, v1, ref maxValue, out tmp)) { return true; }
            //奥
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v4, v5, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v5, v6, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v6, v7, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v7, v4, ref maxValue, out tmp)) { return true; }
            //奥行き
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v0, v4, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v1, v5, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v2, v6, ref maxValue, out tmp)) { return true; }
            if (Interaction.TriangleToLine(tri1, tri2, tri3, v3, v7, ref maxValue, out tmp)) { return true; }

            //三角形の線と、ボクセルの面が交差するならtrue
            //手前
            if (Interaction.RectangleToLine(v0, v1, v2, v3, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v0, v1, v2, v3, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v0, v1, v2, v3, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //奥
            if (Interaction.RectangleToLine(v4, v5, v6, v7, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v4, v5, v6, v7, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v4, v5, v6, v7, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //右
            if (Interaction.RectangleToLine(v1, v5, v6, v2, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v1, v5, v6, v2, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v1, v5, v6, v2, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //左
            if (Interaction.RectangleToLine(v0, v3, v7, v4, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v0, v3, v7, v4, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v0, v3, v7, v4, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //上
            if (Interaction.RectangleToLine(v2, v6, v7, v3, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v2, v6, v7, v3, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v2, v6, v7, v3, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //下
            if (Interaction.RectangleToLine(v0, v4, v5, v1, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v0, v4, v5, v1, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (Interaction.RectangleToLine(v0, v4, v5, v1, tri3, tri1, ref maxValue, out tmp)) { return true; }

            return false;
        }

        /// <summary>
        /// ボクセルの内外判定
        /// </summary>
        private void CalculateInOut()
        {
            Func<Voxel, bool> condition = (voxel) =>
            {
                return voxel?.State == VoxelState.None;
            };
            Action<Voxel> action = (voxel) =>
            {
                voxel.State = VoxelState.Exterior;
            };

            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    var max = Partition - 1;
                    RegionGrowing(Voxels[0, i, j], condition, action);
                    RegionGrowing(Voxels[i, 0, j], condition, action);
                    RegionGrowing(Voxels[i, j, 0], condition, action);
                    RegionGrowing(Voxels[max, i, j], condition, action);
                    RegionGrowing(Voxels[i, max, j], condition, action);
                    RegionGrowing(Voxels[i, j, max], condition, action);
                }
            }

            Action<int, int, int> setInner = (int i, int j, int k) =>
            {
                if (Voxels[i, j, k].State == VoxelState.None)
                {
                    Voxels[i, j, k].State = VoxelState.Inner;
                }
            };

            VoxelAction(setInner);
        }
        #endregion
    }

    /// <summary>
    /// ボクセル
    /// </summary>
    public class Voxel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="xIndex">X要素番号</param>
        /// <param name="yIndex">Y要素番号</param>
        /// <param name="zIndex">Z要素番号</param>
        /// <param name="state">状態</param>
        public Voxel(int xIndex, int yIndex, int zIndex, VoxelState state)
        {
            X = xIndex;
            Y = yIndex;
            Z = zIndex;
            State = state;
        }

        /// <summary>
        /// ボクセルの状態
        /// </summary>
        public VoxelState State { get; set; }

        /// <summary>
        /// Xインデックス
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Yインデックス
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Zインデックス
        /// </summary>
        public int Z { get; private set; }
    }
}
