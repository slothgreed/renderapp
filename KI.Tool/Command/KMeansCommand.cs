using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Renderer;

namespace KI.Tool.Command
{
    /// <summary>
    /// K-Means コマンド
    /// </summary>
    public class KMeansCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// レンダリング形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// クラスタ数
        /// </summary>
        private int clusterNum = 0;

        /// <summary>
        /// 繰り返し回数
        /// </summary>
        private int iterateNum = 0;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="cluster">クラスタ数</param>
        /// <param name="iterate">繰り返し回数</param>
        public KMeansCommand(KIObject asset, int cluster, int iterate)
        {
            renderObject = asset as RenderObject;
            clusterNum = cluster;
            iterateNum = iterate;
        }

        public CommandResult CanExecute(string commandArg)
        {
            return CanCreatePolygon(renderObject);
        }

        public CommandResult Execute(string commandArg)
        {
            var kmeansAlgorithm = new KMeansAlgorithm(renderObject.Polygon as HalfEdgeDS, clusterNum, iterateNum);
            kmeansAlgorithm.Calculate();

            foreach (var cluster in kmeansAlgorithm.Cluster)
            {
                var color = KICalc.RandomColor();
                foreach (var vertex in cluster)
                {
                    vertex.Color = color;
                }
            }

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
