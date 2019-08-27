using KI.Mathmatics;
using OpenTK;
using System;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// シェイプマッチングアルゴリズム
    /// </summary>
    public class ShapeMatchingAlgorithm
    {
        private HalfEdgeDS halfEdgeDS;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="halfEdgeDS">ハーフエッジデータ構造</param>
        public ShapeMatchingAlgorithm(HalfEdgeDS halfEdgeDS)
        {
            // TODO : 引数を頂点情報のみにする.
            this.halfEdgeDS = halfEdgeDS;
            Initialize();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="timeElapsed">ミリ時間差分</param>
        public void Update(float timeMilliElapsed)
        {
            Vector3 sumPosition = Vector3.Zero;
            float sumMass = 0;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                var parameter = vertex.TmpParameter as ShapeMatchingParameter;
                parameter.TmpPosition = (vertex.Position + parameter.Velocity) * timeMilliElapsed;
                sumPosition += parameter.TmpPosition;
                sumMass += parameter.Weight;
            }

            Vector3 averageMass = sumPosition / sumMass;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                var parameter = vertex.TmpParameter as ShapeMatchingParameter;
                parameter.p = parameter.TmpPosition - averageMass;
            }

            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                var parameter = vertex.TmpParameter as ShapeMatchingParameter;
                var Apq = CalcApq();
                var rotate = CalcRotateMatrix(Matrix3Utility.ToMatrix4(Apq));
                vertex.Position = Calculator.Multiply(rotate, parameter.q);
                parameter.UpdatedPoint = vertex.Position;
                parameter.Update(timeMilliElapsed);
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                var parameter = vertex.TmpParameter as ShapeMatchingParameter;
                vertex.Position = parameter.OriginalPosition;
                vertex.TmpParameter = null;
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Initialize()
        {
            Vector3 sumPosition = Vector3.Zero;
            float sumMass = 0;
            float weight = 0.1f;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                if (sumMass == 0)
                {
                    vertex.TmpParameter = new ShapeMatchingParameter(vertex.Position, vertex.Normal, weight);
                }
                else
                {
                    vertex.TmpParameter = new ShapeMatchingParameter(vertex.Position, Vector3.Zero, weight);
                }

                sumPosition += vertex.Position;
                sumMass += weight;
            }

            Vector3 averageMass = sumPosition / sumMass;

            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                var parameter = vertex.TmpParameter as ShapeMatchingParameter;
                parameter.q = vertex.Position - averageMass;
            }
        }

        /// <summary>
        /// Apq行列の計算
        /// </summary>
        /// <returns></returns>
        private Matrix3 CalcApq()
        {
            Matrix3 matrix = Matrix3.Zero;
            foreach (var vertex in halfEdgeDS.HalfEdgeVertexs)
            {
                var parameter = vertex.TmpParameter as ShapeMatchingParameter;
                float weight = parameter.Weight;
                matrix = Vector3Utility.MultipleByTranspose(parameter.p, parameter.q);
                matrix = Matrix3Utility.Multiply(matrix, weight);
            }

            return matrix;
        }

        /// <summary>
        /// 回転マトリクスの計算
        /// </summary>
        /// <param name="Apq"></param>
        /// <returns></returns>
        private Matrix4 CalcRotateMatrix(Matrix4 Apq)
        {
            var transpose = new Matrix4(Apq.Row0, Apq.Row1, Apq.Row2, Apq.Row3);
            transpose.Transpose();
            var multiply = Matrix4Utility.To2dArray(transpose * Apq);
            var sqrtArray = LinearAlgebra.SqrtMatrix(multiply);
            var sqrtMatrix = Matrix4Utility.ToMatrix4(sqrtArray);
            

            try
            {
                sqrtMatrix.Invert();
            }
            catch (Exception)
            {
                sqrtMatrix = Matrix4.Identity;
            }

            return Apq * sqrtMatrix;
        }
        /// <summary>
        /// シェイプマッチングアルゴリズム用のパラメータ
        /// </summary>
        private class ShapeMatchingParameter
        {
            /// <summary>
            /// 終了時に元に戻すための元の頂点位置
            /// </summary>
            public Vector3 OriginalPosition { get; private set; }

            /// <summary>
            /// 力
            /// </summary>
            public Vector3 Velocity { get; private set; }

            /// <summary>
            /// 重さ
            /// </summary>
            public float Weight { get; private set; }

            /// <summary>
            /// p
            /// </summary>
            public Vector3 q { get; set; }

            /// <summary>
            /// q
            /// </summary>
            public Vector3 p { get; set; }

            /// <summary>
            /// 計算用の頂点位置
            /// </summary>
            public Vector3 TmpPosition { get; set; }

            /// <summary>
            /// 更新後の位置
            /// </summary>
            public Vector3 UpdatedPoint { get; set; }
            
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="position">元の頂点位置</param>
            /// <param name="weight">重み</param>
            public ShapeMatchingParameter(Vector3 position, Vector3 velocity, float weight)
            {
                OriginalPosition = position;
                Velocity = velocity;
                Weight = weight;
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            /// <param name="timeElapsed">ミリ時間差分</param>
            public void Update(float timeMilliElapsed)
            {
                Velocity += ((UpdatedPoint - TmpPosition) / timeMilliElapsed) * 0.1f;// + (force / Weight) * timeMilliElapsed;
            }
        }
    }
}
