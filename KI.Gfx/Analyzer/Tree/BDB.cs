using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Utility;
namespace KI.Gfx.Analyzer
{
    public class BDB : IAnalyzer
    {
        #region [Member]
        private Vector3 m_Min;
        private Vector3 m_Max;
        private Vector3 m_Center;
        private Vector3 m_Mean;
        public Vector3 BoundMin
        {
            get { return m_Min; }
        }
        public Vector3 BoundMax
        {
            get { return m_Max; }
        }
        public Vector3 BoundCenter
        {
            get { return m_Center; }
        }
        public Vector3 BoundMean { get { return m_Mean; } }  
        #endregion
        

        public BDB(List<Vector3> position)
        {
            SetBoundBox(position);
        }

        /// <summary>
        /// BDBの算出
        /// </summary>
        private void SetBoundBox(List<Vector3> position)
        {

            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);
            Vector3 sum = new Vector3();
            for (int i = 0; i < position.Count; i++)
            {
                if (min.X > position[i].X) { min.X = position[i].X; }
                if (min.Y > position[i].Y) { min.Y = position[i].Y; }
                if (min.Z > position[i].Z) { min.Z = position[i].Z; }
                if (max.X < position[i].X) { max.X = position[i].X; }
                if (max.Y < position[i].Y) { max.Y = position[i].Y; }
                if (max.Z < position[i].Z) { max.Z = position[i].Z; }
                sum += position[i];
            }
            m_Min = new Vector3(min.X, min.Y, min.Z);
            m_Max = new Vector3(max.X, max.Y, max.Z);
            m_Center = new Vector3((m_Max - m_Min) / 2);
            m_Mean = new Vector3(sum.X / position.Count, sum.Y / position.Count, sum.Z / position.Count);
        }
        /// <summary>
        /// BDBの算出
        /// </summary>
        public static void GetBoundBox(List<Vector3> position, out Vector3 Min, out Vector3 Max)
        {

            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            for (int i = 0; i < position.Count; i++)
            {
                if (min.X > position[i].X) { min.X = position[i].X; }
                if (min.Y > position[i].Y) { min.Y = position[i].Y; }
                if (min.Z > position[i].Z) { min.Z = position[i].Z; }
                if (max.X < position[i].X) { max.X = position[i].X; }
                if (max.Y < position[i].Y) { max.Y = position[i].Y; }
                if (max.Z < position[i].Z) { max.Z = position[i].Z; }
            }
            Min = new Vector3(min.X, min.Y, min.Z);
            Max = new Vector3(max.X, max.Y, max.Z);
        }
        /// <summary>
        /// BDB内に頂点があるか
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <param name="position">チェック頂点</param>
        /// <returns>BDB内=true</returns>
        public static bool CheckInBox(Vector3 min, Vector3 max, Vector3 position)
        {
            if (min.X < position.X && position.X < max.X)
            {
                if (min.Y < position.Y && position.Y < max.Y)
                {
                    if (min.Z < position.Z && position.Z < max.Z)
                    {
                        return true;
                    }

                }

            }
            return false;
        }
        /// <summary>
        /// BDBの三角形群を取得
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static void GetTriPos(Vector3 min, Vector3 max, out List<Vector3> posit, out List<Vector3> normalList)
        {
            normalList = new List<Vector3>();
            posit = new List<Vector3>();
            Vector3 normal;
            Vector3 v0 = new Vector3(min.X, min.Y, min.Z);
            Vector3 v1 = new Vector3(max.X, min.Y, min.Z);
            Vector3 v2 = new Vector3(max.X, max.Y, min.Z);
            Vector3 v3 = new Vector3(min.X, max.Y, min.Z);

            Vector3 v4 = new Vector3(min.X, min.Y, max.Z);
            Vector3 v5 = new Vector3(max.X, min.Y, max.Z);
            Vector3 v6 = new Vector3(max.X, max.Y, max.Z);
            Vector3 v7 = new Vector3(min.X, max.Y, max.Z);

            //手前
            posit.Add(v0); posit.Add(v3); posit.Add(v2); posit.Add(v1);
            normal = GetTriNormal(v0, v3, v2);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //右
            posit.Add(v1); posit.Add(v2); posit.Add(v6); posit.Add(v5);
            normal = GetTriNormal(v1, v2, v6);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //左
            posit.Add(v0); posit.Add(v4); posit.Add(v7); posit.Add(v3);
            normal = GetTriNormal(v0, v4, v7);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //奥
            posit.Add(v4); posit.Add(v5); posit.Add(v6); posit.Add(v7);
            normal = GetTriNormal(v4, v5, v6);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //上
            posit.Add(v2); posit.Add(v3); posit.Add(v7); posit.Add(v6);
            normal = GetTriNormal(v2, v3, v7);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //下
            posit.Add(v1); posit.Add(v5); posit.Add(v4); posit.Add(v0);
            normal = GetTriNormal(v1, v5, v4);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
        }
        /// <summary>
        /// 四角形の取得
        /// </summary>
        private static Vector3 GetTriNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            return KICalc.Normal(v1 - v0, v2 - v0);
        }
        /// <summary>
        /// BDBの三角形群を取得
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public void GetTriPos(out List<Vector3> posit, out List<Vector3> normalList)
        {
            normalList = new List<Vector3>();
            posit = new List<Vector3>();
            Vector3 normal;
            Vector3 v0 = new Vector3(BoundMin.X, BoundMin.Y, BoundMin.Z);
            Vector3 v1 = new Vector3(BoundMax.X, BoundMin.Y, BoundMin.Z);
            Vector3 v2 = new Vector3(BoundMax.X, BoundMax.Y, BoundMin.Z);
            Vector3 v3 = new Vector3(BoundMin.X, BoundMax.Y, BoundMin.Z);

            Vector3 v4 = new Vector3(BoundMin.X, BoundMin.Y, BoundMax.Z);
            Vector3 v5 = new Vector3(BoundMax.X, BoundMin.Y, BoundMax.Z);
            Vector3 v6 = new Vector3(BoundMax.X, BoundMax.Y, BoundMax.Z);
            Vector3 v7 = new Vector3(BoundMin.X, BoundMax.Y, BoundMax.Z);

            //手前
            posit.Add(v0); posit.Add(v3); posit.Add(v2); posit.Add(v1);
            normal = GetTriNormal(v0, v3, v2);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //右
            posit.Add(v1); posit.Add(v2); posit.Add(v6); posit.Add(v5);
            normal = GetTriNormal(v1, v2, v6);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //左
            posit.Add(v0); posit.Add(v4); posit.Add(v7); posit.Add(v3);
            normal = GetTriNormal(v0, v4, v7);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //奥
            posit.Add(v4); posit.Add(v5); posit.Add(v6); posit.Add(v7);
            normal = GetTriNormal(v4, v5, v6);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //上
            posit.Add(v2); posit.Add(v3); posit.Add(v7); posit.Add(v6);
            normal = GetTriNormal(v2, v3, v7);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);

            //下
            posit.Add(v1); posit.Add(v5); posit.Add(v4); posit.Add(v0);
            normal = GetTriNormal(v1, v5, v4);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
            normalList.Add(normal);
        }
       
    }
}
