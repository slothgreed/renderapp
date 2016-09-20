using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.Analyzer
{
    public class Voxel : IAnalyzer
    {
        Vector3 m_Min;
        Vector3 m_Max;
        float m_Length;
        int m_Partition;
        bool[, ,] voxel_Index;
        public List<Vector3> vPosition = new List<Vector3>();
        public List<Vector3> vNormal = new List<Vector3>();
        public Voxel(List<Vector3> position, List<int> posIndex, Matrix4 modelMatrix, int partition)
        {

            m_Partition = partition;
            CalcMinMax(position);
            SetLength(partition);
            voxel_Index = new bool[partition, partition, partition];
            for (int i = 0; i < partition; i++)
            {
                for (int j = 0; j < partition; j++)
                {
                    for (int k = 0; k < partition; k++)
                    {
                        voxel_Index[i, j, k] = false;
                    }
                }
            }
            if (posIndex.Count == 0)
            {
                MakeVoxels(position, modelMatrix);
            }
            else
            {
                List<Vector3> posStream = new List<Vector3>();
                for (int i = 0; i < posIndex.Count / 3; i ++)
                {
                    posStream.Add(position[posIndex[3 * i]]);
                    posStream.Add(position[posIndex[3 * i + 1]]);
                    posStream.Add(position[posIndex[3 * i + 2]]);
                }
                MakeVoxels(posStream, modelMatrix);
            }
            voxel_Index = null;
        }
      
      
        /// <summary>
        /// 形状を包括するBDBで、ボクセルを生成
        /// </summary>
        /// <param name="m_position"></param>
        /// <param name="posIndex"></param>
        private void MakeVoxels(List<Vector3> position,Matrix4 modelMatrix)
        {
            //ボクセルのインデックス番号
            Vector3 vIndex = new Vector3();
            for (int i = 0; i < position.Count / 3; i++)
            {
                Vector3 tri1 = CCalc.Multiply(modelMatrix, position[3 * i]);
                Vector3 tri2 = CCalc.Multiply(modelMatrix, position[3 * i + 1]);
                Vector3 tri3 = CCalc.Multiply(modelMatrix, position[3 * i + 2]);

                //triを包括する部分のボクセルの最小値と最大値のインデックス
                Vector3 minIndex = MinVector(tri1, tri2);
                minIndex = MinVector(minIndex, tri3);

                Vector3 maxIndex = MaxVector(tri1, tri2);
                maxIndex = MaxVector(maxIndex, tri3);

                minIndex -= m_Min;
                maxIndex -= m_Min;
                minIndex /= m_Length;
                maxIndex /= m_Length;

                for (vIndex.X = (int)minIndex.X; vIndex.X < maxIndex.X; vIndex.X++)
                {
                    for (vIndex.Y = (int)minIndex.Y; vIndex.Y < maxIndex.Y; vIndex.Y++)
                    {
                        for (vIndex.Z = (int)minIndex.Z; vIndex.Z < maxIndex.Z; vIndex.Z++)
                        {
                            if (voxel_Index[(int)vIndex.X, (int)vIndex.Y, (int)vIndex.Z])
                            {
                                continue;
                            }
                            if (CheckVoxel(tri1, tri2, tri3, vIndex))
                            {
                                voxel_Index[(int)vIndex.X, (int)vIndex.Y, (int)vIndex.Z] = true;
                                SetVoxel(vIndex);

                            }
                        }
                    }
                }

            }
        }
        

        #region [utility]
        /// <summary>
        /// ボクセルの最小値を返却
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 GetVoxelPosition(Vector3 index)
        {
            Vector3 minVoxel = m_Min;
            minVoxel += index * new Vector3(m_Length);
            return minVoxel;
        }
        /// <summary>
        /// 分割数から長さを計算
        /// </summary>
        /// <param name="partition"></param>
        private void SetLength(int partition)
        {
            float tmpmax, tmpmin;
            tmpmax = m_Max.X;
            if (tmpmax < m_Max.Y) { tmpmax = m_Max.Y; }
            if (tmpmax < m_Max.Z) { tmpmax = m_Max.Z; }

            tmpmin = m_Min.X;
            if (tmpmin > m_Min.Y) { tmpmin = m_Min.Y; }
            if (tmpmin > m_Min.Z) { tmpmin = m_Min.Z; }

            m_Length = (tmpmax - tmpmin) / partition;
        }

        private void CalcMinMax(List<Vector3> position)
        {
            m_Min = position[0];
            m_Max = position[0];
            for (int i = 0; i < position.Count; i++)
            {
                if (m_Min.X > position[i].X) { m_Min.X = position[i].X; }
                if (m_Min.Y > position[i].Y) { m_Min.Y = position[i].Y; }
                if (m_Min.Z > position[i].Z) { m_Min.Z = position[i].Z; }

                if (m_Max.X < position[i].X) { m_Max.X = position[i].X; }
                if (m_Max.Y < position[i].Y) { m_Max.Y = position[i].Y; }
                if (m_Max.Z < position[i].Z) { m_Max.Z = position[i].Z; }

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
            Vector3 maxVoxel = minVoxel + new Vector3(m_Length);

            //点によるチェック
            if (InBox(tri1, minVoxel, maxVoxel)) { return true; }
            if (InBox(tri2, minVoxel, maxVoxel)) { return true; }
            if (InBox(tri3, minVoxel, maxVoxel)) { return true; }


            //ボクセルの線と、三角形の面が交差するならtrue
            float maxValue = float.MaxValue;
            Vector3 tmp = new Vector3();
            //左下手前から反時計周り
            Vector3 v0 = minVoxel;
            Vector3 v1 = new Vector3(minVoxel.X + m_Length, minVoxel.Y, minVoxel.Z);
            Vector3 v2 = new Vector3(minVoxel.X + m_Length, minVoxel.Y + m_Length, minVoxel.Z);
            Vector3 v3 = new Vector3(minVoxel.X, minVoxel.Y + m_Length, minVoxel.Z);

            Vector3 v4 = new Vector3(minVoxel.X, minVoxel.Y, maxVoxel.Z + m_Length);
            Vector3 v5 = new Vector3(minVoxel.X + m_Length, minVoxel.Y, minVoxel.Z + m_Length);
            Vector3 v6 = new Vector3(minVoxel.X + m_Length, minVoxel.Y + m_Length, minVoxel.Z + m_Length);
            Vector3 v7 = new Vector3(minVoxel.X, minVoxel.Y + m_Length, minVoxel.Z + m_Length);

            //手前
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v0, v1, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v1, v2, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v2, v3, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v3, v1, ref maxValue, out tmp)) { return true; }
            //奥
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v4, v5, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v5, v6, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v6, v7, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v7, v4, ref maxValue, out tmp)) { return true; }
            //奥行き
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v0, v4, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v1, v5, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v2, v6, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(tri1, tri2, tri3, v3, v7, ref maxValue, out tmp)) { return true; }

            //三角形の線と、ボクセルの面が交差するならtrue
            //手前
            if (CCalc.CrossPlanetoLinePos(v0, v1, v2, v3, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v0, v1, v2, v3, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v0, v1, v2, v3, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //奥
            if (CCalc.CrossPlanetoLinePos(v4, v5, v6, v7, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v4, v5, v6, v7, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v4, v5, v6, v7, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //右
            if (CCalc.CrossPlanetoLinePos(v1, v5, v6, v2, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v1, v5, v6, v2, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v1, v5, v6, v2, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //左
            if (CCalc.CrossPlanetoLinePos(v0, v3, v7, v4, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v0, v3, v7, v4, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v0, v3, v7, v4, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //上
            if (CCalc.CrossPlanetoLinePos(v2, v6, v7, v3, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v2, v6, v7, v3, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v2, v6, v7, v3, tri3, tri1, ref maxValue, out tmp)) { return true; }
            //下
            if (CCalc.CrossPlanetoLinePos(v0, v4, v5, v1, tri1, tri2, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v0, v4, v5, v1, tri2, tri3, ref maxValue, out tmp)) { return true; }
            if (CCalc.CrossPlanetoLinePos(v0, v4, v5, v1, tri3, tri1, ref maxValue, out tmp)) { return true; }




            
            return false;
        }


        #region [voxel object]
        /// <summary>
        /// ボクセルをvoxelPositionにセットする
        /// </summary>
        private void SetVoxel(Vector3 voxelIndex)
        {
            Vector3 minVoxel = GetVoxelPosition(voxelIndex);
            Vector3 maxVoxel = minVoxel + new Vector3(m_Length);
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
            normal = CCalc.Normal(q1 - q0, q2 - q0);
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
