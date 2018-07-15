﻿using System;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Asset;
using KI.Asset.Attribute;
using OpenTK;
using KI.Mathmatics;

namespace KI.Tool.Command
{
    /// <summary>
    /// K-Means コマンド
    /// </summary>
    public class KMeansCommand : CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="cluster">クラスタ数</param>
        /// <param name="iterate">繰り返し回数</param>
        public KMeansCommand(KMeansCommandArgs commandArg)
            :base(commandArg)
        {
        }

        public override CommandResult CanExecute(CommandArgsBase commandArg)
        {
            return CommandUtility.CanCreatePolygon((commandArg as KMeansCommandArgs).TargetObject);
        }

        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            var kmeansCommandArgs = commandArg as KMeansCommandArgs;
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

            var parentNode = kmeansCommandArgs.Scene.FindNode(targteObject);
            var material = new VertexColorAttribute(
                "KMeansClustering", 
                targteObject.VertexBuffer.ShallowCopy(),
                targteObject.Polygon.Type,
                targteObject.Shader,
                colors);

            targteObject.Attributes.Add(material);
            kmeansCommandArgs.Scene.AddObject(material, parentNode);

            return CommandResult.Success;
        }

        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// KMeans のコマンド
    /// </summary>
    public class KMeansCommandArgs : CommandArgsBase
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
