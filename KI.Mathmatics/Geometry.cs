using System;
using System.Collections.Generic;
using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// 幾何学演算
    /// </summary>
    public static class Geometry
    {


        /// <summary>
        /// 三角形の面積の算出
        /// ヘロンの公式
        /// </summary>
        /// <param name="tri1">頂点1</param>
        /// <param name="tri2">頂点2</param>
        /// <param name="tri3">頂点3</param>
        /// <returns>面積</returns>
        public static float TriangleArea(Vector3 tri1, Vector3 tri2, Vector3 tri3)
        {
            float edge1 = (tri1 - tri2).Length;
            float edge2 = (tri2 - tri3).Length;
            float edge3 = (tri3 - tri1).Length;
            float sum = (edge1 + edge2 + edge3) / 2;

            float value = sum * (sum - edge1) * (sum - edge2) * (sum - edge3);

            return (float)Math.Sqrt(value);
        }

        /// <summary>
        /// 仮想タンジェントベクトルの算出
        /// </summary>
        /// <param name="normal">法線</param>
        /// <param name="tangent1">タンジェント1</param>
        /// <param name="tangent2">タンジェント2</param>
        public static void VirtualTangent(Vector3 normal, out Vector3 tangent1, out Vector3 tangent2)
        {
            if (Math.Abs(normal.X) < Math.Abs(normal.Y))
            {
                tangent1.X = 0;
                tangent1.Y = -normal.Z;
                tangent1.Z = normal.Y;
            }
            else
            {
                tangent1.X = normal.Z;
                tangent1.Y = 0;
                tangent1.Z = -normal.X;
            }

            tangent1.Normalize();
            tangent2 = Vector3.Cross(normal, tangent1);
            tangent2.Normalize();
        }

        /// <summary>
        /// GL_LINESのIndexリストからGL_LINE_LOOPに変換する。
        /// </summary>
        /// <param name="linesIndex">GL_LINES の番号</param>
        /// <param name="lineLoopIndex">GL_LLINE_STRIP の番号</param>
        /// <returns></returns>
        public static bool ConvertLinesToLineLoop(int[] linesIndex, out int[] lineLoopIndex)
        {
            if(linesIndex[0] != linesIndex[linesIndex.Length - 1])
            {
                lineLoopIndex = null;
                return false;
            }
            
            for(int i = 1; i < linesIndex.Length - 1; i+=2)
            {
                if(linesIndex[i] != linesIndex[i + 1])
                {
                    lineLoopIndex = null;
                    return false;   
                }
            }


            lineLoopIndex = new int[linesIndex.Length / 2];
            lineLoopIndex[0] = linesIndex[0];
            for (int i = 1; i < lineLoopIndex.Length; i++)
            {
                lineLoopIndex[i] = linesIndex[(2 * i) - 1];
            }

            return true;
        }

        /// <summary>
        /// LineStripの頂点列から、三角形を作成
        /// </summary>
        /// <param name="lineLoop">位置情報</param>
        /// <param name="orientDir"></param>
        /// <param name="indexList">三角形の構成要素番号</param>
        /// <returns>成功</returns>
        public static bool GenPolygonFromLineLoop(Vector3[] lineLoop, Vector3 orientDir, out int[] indexList)
        {
            if (lineLoop.Length < 2)
            {
                indexList = null;
                return false;
            }

            List<int> candidateList = new List<int>();
            for (int i = 0; i < lineLoop.Length; i++)
            {
                candidateList.Add(i);
            }

            int triangle0 = -1;
            int triangle1 = -1;
            int triangle2 = -1;
            int counter = 0;
            List<int> triangleIndex = new List<int>();
            float area = 0;
            while (true)
            {
                triangle0 = candidateList[counter];
                int triangle1Index = GetNextLoopIndex(counter, 1, candidateList.Count);
                triangle1 = candidateList[triangle1Index];

                int triangle2Index = GetNextLoopIndex(counter, 2, candidateList.Count);
                triangle2 = candidateList[triangle2Index];

                Vector3 center = (lineLoop[triangle0] + lineLoop[triangle2]) * 0.5f;
                if (Inside.Polygon(lineLoop, center) == true)
                {
                    triangleIndex.Add(triangle0);
                    triangleIndex.Add(triangle1);
                    triangleIndex.Add(triangle2);
                    candidateList.Remove(triangle1Index);
                }

                if (candidateList.Count == 2)
                {
                    break;
                }

                counter = GetNextLoopIndex(counter, 1, candidateList.Count);
            }

            Vector3 normal = Calculator.Normal(lineLoop[triangleIndex[0]], lineLoop[triangleIndex[1]], lineLoop[triangleIndex[2]]);
            if (Vector3.Dot(normal, orientDir) > 0)
            {
                triangleIndex.Reverse();
            }

            indexList = triangleIndex.ToArray();

            return true;
        }


        private static int GetNextLoopIndex(int counter,int increment, int listNum)
        {
            counter += increment;
            if (counter < listNum)
            {
                return counter;
            }
            else
            {
                return counter - listNum;
            }
        }
    }
}
