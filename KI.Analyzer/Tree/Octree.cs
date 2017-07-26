using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace KI.Analyzer
{
    public class Octree : IAnalyzer
    {
        Octant m_root;
        private int m_MaxLevel = 0;
        public int MaxLevel { get { return m_MaxLevel; } }

        public Octree(List<Vector3> position,int level)
        {
            m_MaxLevel = level;

            MakeOctree(position);
        }

        #region [Make Octree]
        /// <summary>
        /// オクトリーの作成
        /// </summary>
        private void MakeOctree(List<Vector3> position)
        {
            Vector3 bdbMin;
            Vector3 bdbMax;
            List<Vector3> copy = new List<Vector3>(position);
            BDB.GetBoundBox(copy, out bdbMin, out bdbMax);
            m_root = new Octant(copy, bdbMin, bdbMax, 0);
            MakeOctant(m_root,0);

        }
        /// <summary>
        /// オクタントの作成
        /// </summary>
        /// <param name="octant"></param>
        /// <param name="level"></param>
        private void MakeOctant(Octant parent, int level)
        {
            if (m_MaxLevel == level)
            {
                return;
            }
            List<Vector3> octantMin;
            List<Vector3> octantMax;
            GetOctant(parent.BDBMin, parent.BDBMax, out octantMin, out octantMax);
            List<Vector3>[] octant = new List<Vector3>[8];

            //オクタントリストの生成
            for (int i = 0; i < octant.Length; i++)
            {
                octant[i] = new List<Vector3>();
            }
            //親オクタントの点ループ
            for (int i = 0; i < parent.position.Count; i++)
            {
                //子オクタントに割り当て
                for (int j = 0; j < 8; j++)
                {
                    if (BDB.CheckInBox(octantMin[j], octantMax[j], parent.position[i]))
                    {
                        octant[j].Add(parent.position[i]);
                        break;
                    }
                }
            }
            parent.ClearList();
            level++;

            for (int i = 0; i < octant.Length; i++)
            {
                if (octant[i].Count != 0)
                {
                    Octant child = new Octant(octant[i], octantMin[i], octantMax[i], level);
                    parent.octant[i] = child;
                    MakeOctant(parent.octant[i], level);
                }
            }

        }
        /// <summary>
        /// オクトリーの最小、最大頂点を返す
        /// </summary>
        /// <param name="bdbMin">親オクタントの最小値</param>
        /// <param name="bdbMax">親オクタントの最大値</param>
        /// <param name="octantMin">各オクタント(8個)の最小値</param>
        /// <param name="octantMax">各オクタント(8個)の最大値</param>
        private void GetOctant(Vector3 bdbMin,Vector3 bdbMax,out List<Vector3> octantMin,out List<Vector3> octantMax)
        {
            octantMin = new List<Vector3>();
            octantMax = new List<Vector3>();
            Vector3 center = new Vector3((bdbMin + bdbMax) * 0.5f);


            //左下手前
            octantMin.Add(new Vector3(bdbMin.X, bdbMin.Y, bdbMin.Z));
            octantMax.Add(new Vector3(center.X, center.Y, center.Z));
            //右下手前
            octantMin.Add(new Vector3(center.X, bdbMin.Y, bdbMin.Z));
            octantMax.Add(new Vector3(bdbMax.X, center.Y, center.Z));
            //左上手前
            octantMin.Add(new Vector3(bdbMin.X,center.Y,bdbMin.Z));
            octantMax.Add(new Vector3(center.X,bdbMax.Y,center.Z));
            //右上手前
            octantMin.Add(new Vector3(center.X, center.Y, bdbMin.Z));
            octantMax.Add(new Vector3(bdbMax.X, bdbMax.Y, center.Z));
            
            
            //左下奥
            octantMin.Add(new Vector3(bdbMin.X, bdbMin.Y, center.Z));
            octantMax.Add(new Vector3(center.X, center.Y, bdbMax.Z));
            //右下奥
            octantMin.Add(new Vector3(center.X, bdbMin.Y, center.Z));
            octantMax.Add(new Vector3(bdbMax.X, center.Y, bdbMax.Z));
            //左上奥
            octantMin.Add(new Vector3(bdbMin.X, center.Y, center.Z));
            octantMax.Add(new Vector3(center.X, bdbMax.Y, bdbMax.Z));
            //右上奥
            octantMin.Add(new Vector3(center.X, center.Y, center.Z));
            octantMax.Add(new Vector3(bdbMax.X, bdbMax.Y, bdbMax.Z));
 
        }

        #region [geteer method ]
        /// <summary>
        /// レンダリング位置
        /// </summary>
        /// <param name="posList"></param>
        /// <param name="norList"></param>
        public void GetRenderPosition(out List<Vector3> posList,out List<Vector3> norList)
        {
            List<Octant> leafs = GetLeaf();
            posList = new List<Vector3>();
            norList = new List<Vector3>();
            List<Vector3> posit = new List<Vector3>();
            List<Vector3> normal = new List<Vector3>();
            for (int i = 0; i < leafs.Count; i++)
            {
                BDB.GetTriPos(leafs[i].BDBMin, leafs[i].BDBMax, out posit, out normal);
                posList.AddRange(posit);
                norList.AddRange(normal);
            }
            
        }
        /// <summary>
        /// 葉の取得
        /// </summary>
        /// <returns></returns>
        public List<Octant> GetLeaf()
        {
            List<Octant> leafs = new List<Octant>();
            SearchLeaf(leafs, m_root);
            return leafs;
        }
        #endregion

        #region [option method]
        /// <summary>
        /// 葉ノードの探索
        /// </summary>
        /// <param name="leafs"></param>
        /// <param name="node"></param>
        private void SearchLeaf(List<Octant> leafs, Octant node)
        {

            if (IsLeaf(node))
            {
                leafs.Add(node);
            }
            else
            {
                for (int i = 0; i < node.octant.Length; i++)
                {
                    if (node.octant[i] != null)
                    {
                        SearchLeaf(leafs, node.octant[i]);
                    }

                }

            }

        }
        /// <summary>
        /// 葉ノードかチェック
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsLeaf(Octant node)
        {
            for (int i = 0; i < node.octant.Length; i++)
            {
                if (node.octant[i] != null)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
        
        
        #endregion
        
    }


    public class Octant
    {
        //左下、右下、左上、右上その後奥の順
        public Octant[] octant = new Octant[8];

        private int m_Level;
        private Vector3 m_bdbMin;
        private Vector3 m_bdbMax;
        private List<Vector3> m_position;

        public int Level { get { return m_Level; } }
        public Vector3 BDBMin { get { return m_bdbMin; } }
        public Vector3 BDBMax { get { return m_bdbMax; } }

        public List<Vector3> position { get { return m_position; } }
        
        public Octant(List<Vector3> position,Vector3 min,Vector3 max,int level)
        {
            m_bdbMax = max;
            m_bdbMin = min;
            m_position = position;
            m_Level = level;
        }

        public void ClearList()
        {
            m_position.Clear();
        }
    }
}
