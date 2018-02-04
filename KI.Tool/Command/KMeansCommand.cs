﻿using System;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Renderer;
using KI.Renderer.Attribute;
using OpenTK;

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
        /// シーン
        /// </summary>
        private Scene scene;

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
        /// <param name="scene">シーン</param>
        /// <param name="asset">作成するオブジェクト</param>
        /// <param name="cluster">クラスタ数</param>
        /// <param name="iterate">繰り返し回数</param>
        public KMeansCommand(Scene scene, KIObject asset, int cluster, int iterate)
        {
            renderObject = asset as RenderObject;
            clusterNum = cluster;
            iterateNum = iterate;
            this.scene = scene;
        }

        public CommandResult CanExecute(string commandArg)
        {
            return CanCreatePolygon(renderObject);
        }

        public CommandResult Execute(string commandArg)
        {
            var halfEdgeDS = renderObject.Polygon as HalfEdgeDS;
            var kmeansAlgorithm = new KMeansAlgorithm(halfEdgeDS, clusterNum, iterateNum);
            kmeansAlgorithm.Calculate();

            var colors = new Vector3[halfEdgeDS.Vertexs.Count];
            foreach (var cluster in kmeansAlgorithm.Cluster)
            {
                var color = KICalc.RandomColor();
                foreach (var vertex in cluster)
                {
                    colors[vertex.Index] = color;
                }
            }

            var parentNode = scene.FindNode(renderObject);
            var material = new VertexColorAttribute(
                "KMeansClustering", 
                renderObject.GeometryAttribute.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type, 
                renderObject.Shader, colors);

            renderObject.Attributes.Add(material);
            scene.AddObject(material, parentNode);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
