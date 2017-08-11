using System;
using System.Collections.Generic;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Analyzer.Algorithm.MarchingCube
{
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

    public class MarchingVoxel
    {
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

        public float NeightNum { get; set; }

        public Dictionary<CubeVertex, float> Neight { get; set; }
    }

    public class MarchingCubesAlgorithm
    {
        private float Size;

        private int Partition;

        public MarchingVoxel[,,] MarchingSpace;

        /// <summary>
        /// MarchingTrianlge情報
        /// </summary>
        public List<Vector3> PositionList = new List<Vector3>();

        /// <summary>
        /// MarchingTrianlge情報
        /// </summary>
        public List<Vector3> ColorList = new List<Vector3>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="voxel">voxel</param>
        public MarchingCubesAlgorithm(float size, int partition)
        {
            Size = size;
            Partition = partition;
            CalculateSpace();
            CreateTriangle();
        }

        private float Interval
        {
            get
            {
                return (float)(Size / Partition);
            }
        }

        public Vector3 Min
        {
            get
            {
                return Vector3.Zero;
            }
        }

        public float Threshold
        {
            get
            {
                return SphereSize;
            }
        }

        /// <summary>
        /// 陰関数球のサイズ
        /// </summary>
        public float SphereSize
        {
            get
            {
                return Size / 7;
            }
        }

        /// <summary>
        /// 陰関数球の中心
        /// </summary>
        public Vector3 SphereCenter
        {
            get
            {
                return new Vector3(Size / 2);
            }
        }

        public Vector3 VertexCreatePosition(int x, int y, int z, int v0, int v1)
        {
            var marching = MarchingSpace[x, y, z];

            CubeVertex ver0 = (CubeVertex)v0;
            CubeVertex ver1 = (CubeVertex)v1;

            var pos0 = GetPosition(x, y, z, ver0);
            var pos1 = GetPosition(x, y, z, ver1);
            //return (pos0 + pos1) / 2;

            if (Math.Abs(Threshold - marching.Neight[ver0]) < KICalc.THRESHOLD05)
            {
                return pos0;
            }

            if (Math.Abs(Threshold - marching.Neight[ver1]) < KICalc.THRESHOLD05)
            {
                return pos1;
            }

            var mu = (Threshold - marching.Neight[ver0]) / (marching.Neight[ver1] - marching.Neight[ver0]);

            return pos0 + mu * (pos1 - pos0);
        }

        /// <summary>
        /// 頂点位置の算出
        /// </summary>
        public void CreateTriangle()
        {
            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        if (MarchingSpace[i, j, k].State == 0)
                        {
                            continue;
                        }

                        var vertexList = new Vector3[12];
                        var marching = MarchingSpace[i, j, k];

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
                            ColorList.Add(new Vector3(0.7f));
                            ColorList.Add(new Vector3(0.7f));
                            ColorList.Add(new Vector3(0.7f));
                            PositionList.Add(vertexList[MarchingTable.TriIndexTable[marching.State, l]]);
                            PositionList.Add(vertexList[MarchingTable.TriIndexTable[marching.State, l + 2]]);
                            PositionList.Add(vertexList[MarchingTable.TriIndexTable[marching.State, l + 1]]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 空間の取得Voxelより一回り大きい形状一回り大きいのは必ずNULL
        /// </summary>
        public void CalculateSpace()
        {
            MarchingSpace = new MarchingVoxel[Partition, Partition, Partition];

            var inter = Size / 4;
            var value = new float[8];
            for (int i = 0; i < Partition; i++)
            {
                for (int j = 0; j < Partition; j++)
                {
                    for (int k = 0; k < Partition; k++)
                    {
                        MarchingSpace[i, j, k] = new MarchingVoxel();

                        for (int l = 0; l < (int)CubeVertex.Num; l++)
                        {
                            var pos = GetPosition(i, j, k, (CubeVertex)l);

                            value[0] = (pos - new Vector3(inter, inter, inter)).Length - SphereSize;
                            value[1] = (pos - new Vector3(inter, inter * 3, inter)).Length - SphereSize;
                            value[2] = (pos - new Vector3(inter * 3, inter, inter)).Length - SphereSize;
                            value[3] = (pos - new Vector3(inter * 3, inter * 3, inter)).Length - SphereSize;

                            value[4] = (pos - new Vector3(inter, inter, inter * 3)).Length - SphereSize;
                            value[5] = (pos - new Vector3(inter, inter * 3, inter * 3)).Length - SphereSize;
                            value[6] = (pos - new Vector3(inter * 3, inter, inter * 3)).Length - SphereSize;
                            value[7] = (pos - new Vector3(inter * 3, inter * 3, inter * 3)).Length - SphereSize;

                            var marching = MarchingSpace[i, j, k];
                            for (int x = 0; x < value.Length; x++)
                            {
                                marching.Neight[(CubeVertex)l] = Math.Min(marching.Neight[(CubeVertex)l], value[x]);
                            }

                            if (marching.Neight[(CubeVertex)l] < SphereSize)
                            {
                                marching.State |= (int)Math.Pow(2, l);
                                marching.NeightNum++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ボクセルの最小値を返却
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 GetPosition(int x, int y, int z, CubeVertex state)
        {
            Vector3 position = new Vector3(Min);

            position.X += x * Interval;
            position.Y += y * Interval;
            position.Z += z * Interval;

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

        private void CreateQuad(int i, int j, int k)
        {
            var marching = MarchingSpace[i, j, k];

            var color = new Vector3();

            switch ((int)marching.NeightNum)
            {
                case 1:
                    color = Vector3.UnitX;
                    return;
                case 2:
                    color = Vector3.UnitY;
                    return;
                case 3:
                    color = Vector3.UnitZ;
                    break;
                case 4:
                    color = Vector3.UnitX + Vector3.UnitY;
                    break;
                case 5:
                    color = Vector3.UnitX + Vector3.UnitZ;
                    break;
                case 6:
                    color = Vector3.UnitY + Vector3.UnitZ;
                    break;
                case 7:
                    color = Vector3.UnitX + Vector3.UnitY + Vector3.UnitZ;
                    break;
                default:
                    return;
            }
            //return;
            for (int x = 0; x < 36; x++)
            {
                ColorList.Add(color);
            }

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)4));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)5));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)6));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)4));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)6));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)7));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)5));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)1));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)2));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)5));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)2));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)6));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)6));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)2));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)3));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)6));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)3));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)7));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)7));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)3));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)0));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)7));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)0));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)4));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)4));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)0));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)1));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)4));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)1));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)5));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)0));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)3));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)2));

            PositionList.Add(GetPosition(i, j, k, (CubeVertex)0));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)2));
            PositionList.Add(GetPosition(i, j, k, (CubeVertex)1));
        }
    }
}
