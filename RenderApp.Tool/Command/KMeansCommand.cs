using System;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset.Attribute;
using KI.Mathmatics;
using KI.Renderer;
using KI.Tool.Command;
using OpenTK;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// K-Means コマンド
    /// </summary>
    public class KMeansCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private KMeansCommandArgs kmeansCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public KMeansCommand(KMeansCommandArgs commandArg)
        {
            kmeansCommandArgs = commandArg;
        }

        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(kmeansCommandArgs.TargetObject);
        }

        public override CommandResult Execute()
        {
            var halfEdgeDS = kmeansCommandArgs.TargetObject.Polygon as HalfEdgeDS;
            var kmeansAlgorithm = new KMeansAlgorithm(halfEdgeDS, kmeansCommandArgs.ClusterNum, kmeansCommandArgs.IterateNum);
            kmeansAlgorithm.Calculate();

            var colors = new Vector3[halfEdgeDS.Vertexs.Count];
            foreach (var cluster in kmeansAlgorithm.Cluster)
            {
                var color = RandamValue.Color();
                foreach (var vertex in cluster)
                {
                    colors[vertex.Index] = color;
                }
            }

            var targteObject = kmeansCommandArgs.TargetObject;

            var material = new VertexColorAttribute(
                "KMeansClustering", 
                targteObject.VertexBuffer.ShallowCopy(),
                targteObject.Type,
                targteObject.Shader,
                colors);

            targteObject.Attributes.Add(material);

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// KMeans のコマンド
    /// </summary>
    public class KMeansCommandArgs
    {
        /// <summary>
        /// 対象形状
        /// </summary>
        public RenderObject TargetObject;

        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene;

        /// <summary>
        /// クラスタ数
        /// </summary>
        public int ClusterNum = 0;

        /// <summary>
        /// 繰り返し回数
        /// </summary>
        public int IterateNum = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetObject">対象形状</param>
        /// <param name="scene">シーン</param>
        /// <param name="clusterNum">クラスタ数</param>
        /// <param name="iterateNum">繰り返し回数</param>
        public KMeansCommandArgs(RenderObject targetObject, Scene scene, int clusterNum, int iterateNum)
        {
            TargetObject = targetObject;
            Scene = scene;
            ClusterNum = clusterNum;
            IterateNum = iterateNum;
        }
    }
}
