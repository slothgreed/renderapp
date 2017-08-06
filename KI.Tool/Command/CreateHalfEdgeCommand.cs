﻿using System;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;

namespace KI.Tool.Command
{
    /// <summary>
    /// ハーフエッジの作成
    /// </summary>
    public class CreateHalfEdgeCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateHalfEdgeCommand(KIObject asset)
        {
            renderObject = asset as RenderObject;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState CanExecute(string commandArg)
        {
            return CanCreateGeometry(renderObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Execute(string commandArg)
        {
            var half = AssetFactory.Instance.CreateHalfEdge(renderObject.Geometry.GeometryInfo.Position, renderObject.Geometry.GeometryInfo.Index);
            RenderObject halfEdge = RenderObjectFactory.Instance.CreateRenderObject("HalfEdge :" + renderObject.Name);
            halfEdge.SetGeometryInfo(half.Geometrys[0]);
            halfEdge.ModelMatrix = renderObject.ModelMatrix;
            Global.Scene.AddObject(halfEdge);

            return CommandState.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandState Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
