using System;
using System.Collections.Generic;
using System.Linq;

namespace KI.Analyzer.Tree
{
    /// <summary>
    /// 距離場
    /// </summary>
    public class DistanceField
    {
        /// <summary>
        /// ボクセル空間
        /// </summary>
        private VoxelSpace voxelSpace;

        /// <summary>
        /// 距離場
        /// </summary>
        private float[,,] distanceField;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="voxel">ボクセル</param>
        public DistanceField(VoxelSpace voxel)
        {
            voxelSpace = voxel;
            distanceField = new float[voxel.Partition, voxel.Partition, voxel.Partition];
        }

        #region [calculate distance field]

        /// <summary>
        /// 距離場の算出開始
        /// </summary>
        /// <returns>開始ボクセル</returns>
        private Voxel StartDistanceFieldVoxel()
        {
            Func<int, int, int, bool> checkInner = (i, j, k) =>
            {
                var voxel = voxelSpace.GetVoxel(i, j, k);
                if (voxel?.State == VoxelState.Inner &&
                distanceField[voxel.X, voxel.Y, voxel.Z] == float.MaxValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };

            Func<int, int, int, Voxel> neightInner = (int i, int j, int k) =>
            {
                if (voxelSpace.Voxels[i, j, k].State == VoxelState.Border)
                {
                    var hit = voxelSpace.SearchNeightVoxel(voxelSpace.Voxels[i, j, k], checkInner, NeightType.All);
                    if (hit.Any())
                    {
                        return hit.FirstOrDefault();
                    }
                }

                return null;
            };

            return voxelSpace.SearchAllVoxel(neightInner);
        }

        /// <summary>
        /// 距離場の算出duckでやるとバグる
        /// 周囲がInnerで未決定の場合、事前に決まった値の加算値になる。
        /// 境界から、Closingして求めるほうがましになる。
        /// </summary>
        private void CalculateDistanceField()
        {
            var inners = new List<Voxel>();
            Action<int, int, int> initializeField = (int i, int j, int k) =>
            {
                if (voxelSpace.Voxels[i, j, k].State == VoxelState.Inner)
                {
                    distanceField[i, j, k] = float.MaxValue;
                    inners.Add(voxelSpace.Voxels[i, j, k]);
                }
                else
                {
                    distanceField[i, j, k] = 0;
                }
            };

            voxelSpace.VoxelAction(initializeField);

            while (true)
            {
                Voxel start = StartDistanceFieldVoxel();
                if (start == null)
                {
                    return;
                }

                Func<Voxel, bool> condition = (voxel) =>
                {
                    if (voxel.State == VoxelState.Inner &&
                        distanceField[voxel.X, voxel.Y, voxel.Z] == float.MaxValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };

                Func<int, int, int, Voxel> minValue = (i, j, k) =>
                {
                    return null;
                };

                Action<Voxel> action = (voxel) =>
                {
                    float min = float.MaxValue;
                    Voxel minVoxel = null;
                    var neight = voxelSpace.GetVoxel(voxel.X + 1, voxel.Y, voxel.Z);
                    if (neight != null && GetDistanceValue(neight) < min)
                    {
                        min = GetDistanceValue(neight);
                        minVoxel = neight;
                    }

                    neight = voxelSpace.GetVoxel(voxel.X, voxel.Y + 1, voxel.Z);
                    if (neight != null && GetDistanceValue(neight) < min)
                    {
                        min = GetDistanceValue(neight);
                        minVoxel = neight;
                    }

                    neight = voxelSpace.GetVoxel(voxel.X, voxel.Y, voxel.Z + 1);
                    if (neight != null && GetDistanceValue(neight) < min)
                    {
                        min = GetDistanceValue(neight);
                        minVoxel = neight;
                    }

                    neight = voxelSpace.GetVoxel(voxel.X - 1, voxel.Y, voxel.Z);
                    if (neight != null && GetDistanceValue(neight) < min)
                    {
                        min = GetDistanceValue(neight);
                        minVoxel = neight;
                    }

                    neight = voxelSpace.GetVoxel(voxel.X, voxel.Y - 1, voxel.Z);
                    if (neight != null && GetDistanceValue(neight) < min)
                    {
                        min = GetDistanceValue(neight);
                        minVoxel = neight;
                    }

                    neight = voxelSpace.GetVoxel(voxel.X, voxel.Y, voxel.Z - 1);
                    if (neight != null && GetDistanceValue(neight) < min)
                    {
                        min = GetDistanceValue(neight);
                        minVoxel = neight;
                    }

                    if (minVoxel != null)
                    {
                        distanceField[voxel.X, voxel.Y, voxel.Z] = (voxelSpace.GetVoxelPosition(voxel) - voxelSpace.GetVoxelPosition(minVoxel)).Length + GetDistanceValue(minVoxel);
                    }
                };

                voxelSpace.RegionGrowing(start, condition, action);
            }
        }

        /// <summary>
        /// ボクセルから距離値を取得
        /// </summary>
        /// <param name="voxel">ボクセル</param>
        /// <returns>距離値</returns>
        private float GetDistanceValue(Voxel voxel)
        {
            return distanceField[voxel.X, voxel.Y, voxel.Z];
        }

        #endregion
    }
}
