using System;
using System.Collections.Generic;
using KI.Analyzer.Algorithm.MarchingCube;
using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// マーチングキューブ用のボクセル
    /// </summary>
    public class MarchingVoxel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MarchingVoxel()
        {
            State = 0;
            Neight = new Dictionary<CubeVertex, float>();

            Neight.Add(CubeVertex.xyZ, float.MaxValue);
            Neight.Add(CubeVertex.XyZ, float.MaxValue);
            Neight.Add(CubeVertex.Xyz, float.MaxValue);
            Neight.Add(CubeVertex.xyz, float.MaxValue);
            Neight.Add(CubeVertex.xYZ, float.MaxValue);
            Neight.Add(CubeVertex.XYZ, float.MaxValue);
            Neight.Add(CubeVertex.XYz, float.MaxValue);
            Neight.Add(CubeVertex.xYz, float.MaxValue);
        }

        /// <summary>
        /// 0～15　MarchingCubesの形状
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 隣接ボクセルの値
        /// </summary>
        public Dictionary<CubeVertex, float> Neight { get; set; }
    }

    /// <summary>
    /// マーチンキューブアルゴリズム
    /// </summary>
    public class MarchingCubesAlgorithm
    {
        /// <summary>
        /// 閾値
        /// </summary>
        private float isoLevel = 0;

        /// <summary>
        /// ボクセル空間
        /// </summary>
        private VoxelSpace voxelSpace;

        /// <summary>
        /// マーチングキューブ用のボクセル空間
        /// </summary>
        private MarchingVoxel[,,] marchingSpace;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="voxelSpace">ボクセル空間</param>
        /// <param name="isoLevel">閾値</param>
        public MarchingCubesAlgorithm(VoxelSpace voxelSpace, float isoLevel)
        {
            this.isoLevel = isoLevel;
            this.voxelSpace = voxelSpace;
            marchingSpace = new MarchingVoxel[voxelSpace.Partition, voxelSpace.Partition, voxelSpace.Partition];

            CalculateSpace();
            CreateTriangle();
        }

        /// <summary>
        /// MarchingTrianlge情報
        /// </summary>
        public List<Mesh> Meshs { get; private set; } = new List<Mesh>();

        /// <summary>
        /// 頂点の作成位置の取得
        /// </summary>
        /// <param name="xIndex">xの要素番号</param>
        /// <param name="yIndex">yの要素番号</param>
        /// <param name="zIndex">zの要素番号</param>
        /// <param name="vertexPosition0">ボクセルの頂点位置0</param>
        /// <param name="vertexPosition1">ボクセルの頂点位置1</param>
        /// <returns>算出結果</returns>
        private Vector3 VertexCreatePosition(int xIndex, int yIndex, int zIndex, int vertexPosition0, int vertexPosition1)
        {
            var marching = marchingSpace[xIndex, yIndex, zIndex];

            CubeVertex ver0 = (CubeVertex)vertexPosition0;
            CubeVertex ver1 = (CubeVertex)vertexPosition1;

            var pos0 = voxelSpace.GetPosition(xIndex, yIndex, zIndex, ver0);
            var pos1 = voxelSpace.GetPosition(xIndex, yIndex, zIndex, ver1);
            //return (pos0 + pos1) / 2;

            if (Math.Abs(isoLevel - marching.Neight[ver0]) < Calculator.THRESHOLD05)
            {
                return pos0;
            }

            if (Math.Abs(isoLevel - marching.Neight[ver1]) < Calculator.THRESHOLD05)
            {
                return pos1;
            }

            if (Math.Abs(marching.Neight[ver1] - marching.Neight[ver0]) < Calculator.THRESHOLD05)
            {
                return pos1;
            }

            var mu = (isoLevel - marching.Neight[ver0]) / (marching.Neight[ver1] - marching.Neight[ver0]);
            // 内分点
            return pos0 + mu * (pos1 - pos0);
        }

        /// <summary>
        /// 頂点位置の算出
        /// </summary>
        private void CreateTriangle()
        {
            Func<int, int, int, bool> createTriCondition = (i, j, k) =>
               {
                   return marchingSpace[i, j, k].State == 0;
               };

            Action<int, int, int> createTriAction = (int i, int j, int k) =>
              {
                  var vertexList = new Vector3[12];
                  var marching = marchingSpace[i, j, k];

                  if ((MarchingTable.EdgeTable[marching.State] & 1) != 0)
                      vertexList[0] = VertexCreatePosition(i, j, k, 0, 1);
                  if ((MarchingTable.EdgeTable[marching.State] & 2) != 0)
                      vertexList[1] = VertexCreatePosition(i, j, k, 1, 2);
                  if ((MarchingTable.EdgeTable[marching.State] & 4) != 0)
                      vertexList[2] = VertexCreatePosition(i, j, k, 2, 3);
                  if ((MarchingTable.EdgeTable[marching.State] & 8) != 0)
                      vertexList[3] = VertexCreatePosition(i, j, k, 3, 0);
                  if ((MarchingTable.EdgeTable[marching.State] & 16) != 0)
                      vertexList[4] = VertexCreatePosition(i, j, k, 4, 5);
                  if ((MarchingTable.EdgeTable[marching.State] & 32) != 0)
                      vertexList[5] = VertexCreatePosition(i, j, k, 5, 6);
                  if ((MarchingTable.EdgeTable[marching.State] & 64) != 0)
                      vertexList[6] = VertexCreatePosition(i, j, k, 6, 7);
                  if ((MarchingTable.EdgeTable[marching.State] & 128) != 0)
                      vertexList[7] = VertexCreatePosition(i, j, k, 7, 4);
                  if ((MarchingTable.EdgeTable[marching.State] & 256) != 0)
                      vertexList[8] = VertexCreatePosition(i, j, k, 0, 4);
                  if ((MarchingTable.EdgeTable[marching.State] & 512) != 0)
                      vertexList[9] = VertexCreatePosition(i, j, k, 1, 5);
                  if ((MarchingTable.EdgeTable[marching.State] & 1024) != 0)
                      vertexList[10] = VertexCreatePosition(i, j, k, 2, 6);
                  if ((MarchingTable.EdgeTable[marching.State] & 2048) != 0)
                      vertexList[11] = VertexCreatePosition(i, j, k, 3, 7);

                  for (int l = 0; MarchingTable.TriIndexTable[marching.State, l] != -1; l += 3)
                  {
                      Meshs.Add(
                          new Mesh(
                              new Vertex(3 * Meshs.Count, vertexList[MarchingTable.TriIndexTable[marching.State, l]], Vector3.UnitY),
                              new Vertex(3 * Meshs.Count + 1, vertexList[MarchingTable.TriIndexTable[marching.State, l + 2]], Vector3.UnitY),
                              new Vertex(3 * Meshs.Count + 2, vertexList[MarchingTable.TriIndexTable[marching.State, l + 1]], Vector3.UnitY)));
                  }
              };

            voxelSpace.VoxelAction(createTriCondition, createTriAction);
        }

        /// <summary>
        /// 空間の取得Voxelより一回り大きい形状一回り大きいのは必ずNULL
        /// </summary>
        private void CalculateSpace()
        {
            var value = new float[8];
            Action<int, int, int> action = (int i, int j, int k) =>
              {
                  marchingSpace[i, j, k] = new MarchingVoxel();
                    var marching = marchingSpace[i, j, k];

                  marching.Neight[CubeVertex.xyZ] = VoxelState(i, j, k + 1);
                  marching.Neight[CubeVertex.XyZ] = VoxelState(i + 1, j, k + 1);
                  marching.Neight[CubeVertex.Xyz] = VoxelState(i + 1, j, k);
                  marching.Neight[CubeVertex.xyz] = VoxelState(i, j, k);

                  marching.Neight[CubeVertex.xYZ] = VoxelState(i, j + 1, k + 1);
                  marching.Neight[CubeVertex.XYZ] = VoxelState(i + 1, j + 1, k + 1);
                  marching.Neight[CubeVertex.XYz] = VoxelState(i + 1, j + 1, k);
                  marching.Neight[CubeVertex.xYz] = VoxelState(i, j + 1, k);

                  for (int l = 0; l < (int)CubeVertex.Num; l++)
                  {
                      if (marching.Neight[(CubeVertex)l] < isoLevel)
                      {
                          marching.State |= (int)Math.Pow(2, l);
                      }
                  }
              };

            voxelSpace.VoxelAction(action);
        }

        /// <summary>
        /// 仮実装
        /// </summary>
        /// <param name="xIndex">xの要素番号</param>
        /// <param name="yIndex">yの要素番号</param>
        /// <param name="zIndex">zの要素番号</param>
        /// <returns>値</returns>
        private float VoxelState(int xIndex, int yIndex, int zIndex)
        {
            var voxel = voxelSpace.GetVoxel(xIndex, yIndex, zIndex);
            if (voxel == null)
            {
                return 1;
            }

            if (voxel.State == Analyzer.VoxelState.Border)
            {
                return 0.5f;
            }

            return 1;
        }
    }
}
