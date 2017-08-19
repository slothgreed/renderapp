using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// kdtree
    /// </summary>
    public class Kdtree
    {
        #region [Member]

        /// <summary>
        /// ルートノード
        /// </summary>
        private KdList rootNode;

        /// <summary>
        /// 最大階層
        /// </summary>
        private int maxLevel;

        #endregion
        #region [Constructor]
        /// <summary>
        /// Kdtreeの分割数
        /// </summary>
        /// <param name="position"></param>
        public Kdtree(List<Vector3> position, int level)
        {
            maxLevel = level;
            MakeKdtree(position, level);
        }
        #endregion
        #region [Getter Method]
        /// <summary>
        /// Kdtreeの葉のBDB取得
        /// </summary>
        /// <returns></returns>
        public void GetRenderPosit(out List<Vector3> bdbPosition, out List<Vector3> bdbNormal)
        {
            bdbPosition = new List<Vector3>();
            bdbNormal = new List<Vector3>();
            List<Vector3> posit = new List<Vector3>();
            List<Vector3> normal = new List<Vector3>();
            List<KdList> leaf = GetLeaf();
            for (int i = 0; i < leaf.Count; i++)
            {
                leaf[i].GetTriPos(out posit, out normal);
                bdbPosition.AddRange(posit);
                bdbNormal.AddRange(normal);
            }
        }

        /// <summary>
        /// 木の探索
        /// </summary>
        public List<KdList> GetLeaf()
        {
            List<KdList> leaf = new List<KdList>();
            KdList list = rootNode;
            SearchLeaf(leaf, rootNode);
            return leaf;
        }
        #endregion
        #region [MakeKdTree]
        /// <summary>
        /// Kdtreeを作る
        /// </summary>
        private void MakeKdtree(List<Vector3> position, int partition)
        {
            List<Vector3> copy = new List<Vector3>(position);

            //1個目のツリー作成
            rootNode = new KdList(copy, 0);
            int level = 0;
            PartitionPoint(rootNode, level);
        }

        /// <summary>
        /// 点を分ける再起関数
        /// </summary>
        private void PartitionPoint(KdList kdlist, int level)
        {
            if (kdlist.Level == maxLevel)
            {
                return;
            }

            Vector3 axisNormal = GetAxisNormal(level);
            Vector3 axisPos = GetAxisPos(kdlist);

            Vector3 vector;
            int currentLevel = level;
            List<Vector3> list1 = new List<Vector3>();
            List<Vector3> list2 = new List<Vector3>();

            for (int i = 0; i < kdlist.Position.Count; i++)
            {
                vector = axisPos - kdlist.Position[i];
                if (Vector3.Dot(vector, axisNormal) > 0)
                {
                    list1.Add(new Vector3(kdlist.Position[i]));
                }
                else
                {
                    list2.Add(new Vector3(kdlist.Position[i]));
                }
            }

            kdlist.ClearList();
            level++;
            if (list1.Count != 0)
            {
                KdList kdlist1 = new KdList(list1, level);
                kdlist.Left = kdlist1;
                PartitionPoint(kdlist1, level);
            }

            if (list2.Count != 0)
            {
                KdList kdlist2 = new KdList(list2, level);
                kdlist.Right = kdlist2;
                PartitionPoint(kdlist2, level);
            }
        }

        /// <summary>
        /// 軸位置を返す
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private Vector3 GetAxisPos(KdList list)
        {
            return list.Mean;
        }

        /// <summary>
        /// 分割面の法線方向のゲッタ
        /// </summary>
        /// <returns></returns>
        private Vector3 GetAxisNormal(int level)
        {
            int number = level % 3;
            switch (number)
            {
                case 0:
                    return Vector3.UnitX;
                case 1:
                    return Vector3.UnitY;
                case 2:
                    return Vector3.UnitZ;
            }

            return Vector3.Zero;
        }
        #endregion
        #region [Option Method]
        /// <summary>
        /// 葉の探索再帰関数
        /// </summary>
        /// <param name="leaf">葉が格納されているリスト</param>
        /// <param name="kdlist"></param>
        private void SearchLeaf(List<KdList> leaf, KdList kdlist)
        {
            if (kdlist.Left == null && kdlist.Right == null)
            {
                leaf.Add(kdlist);
            }
            else
            {
                if (kdlist.Left != null)
                {
                    SearchLeaf(leaf, kdlist.Left);
                }

                if (kdlist.Right != null)
                {
                    SearchLeaf(leaf, kdlist.Right);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Kd-treeのリスト
    /// </summary>
    public class KdList : BDB
    {
        public KdList(List<Vector3> posit, int level)
            : base(posit)
        {
            Level = level;
            Position = posit;
        }

        /// <summary>
        /// 階層のレベル
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 位置情報
        /// </summary>
        public List<Vector3> Position { get; private set; }

        /// <summary>
        /// 左の枝
        /// </summary>
        public KdList Left { get; set; }

        /// <summary>
        /// 右の枝
        /// </summary>
        public KdList Right { get; set; }

        /// <summary>
        /// 位置情報の削除
        /// </summary>
        public void ClearList()
        {
            Position.Clear();
        }
    }
}
