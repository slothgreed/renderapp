using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Gfx.Geometry;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// パーセプトロンコマンド
    /// </summary>
    public class PerceptronCommand : ICommand
    {
        /// <summary>
        /// シーン
        /// </summary>
        private Scene scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        public PerceptronCommand(Scene scene)
        {
            this.scene = scene;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult CanExecute(string commandArg)
        {
            return CommandResult.Success;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult Execute(string commandArg)
        {
            var values = new float[100][];
            var result = new float[100];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new float[3];
            }


            var rand = new Random();
            for (int i = 0; i < values.Length; i++)
            {
                if (i < values.Length / 2)
                {
                    values[i][0] = rand.Next(0, 100);
                    values[i][1] = rand.Next(0, 100);
                    values[i][2] = rand.Next(0, 100);
                    result[i] = 1;
                }
                else
                {
                    values[i][0] = rand.Next(-100, 0);
                    values[i][1] = rand.Next(-100, 0);
                    values[i][2] = rand.Next(-100, 0);
                    result[i] = -1;
                }
            }

            var perceptron = new PerceptronAlgorithm(1, 100, values, result);
            perceptron.Calculate();
            List<Vertex> vertexs = new List<Vertex>();
            for (int i = 0; i < values.Length; i++)
            {
                var label = perceptron.Predict(values[i], perceptron.Weight);

                if (label == 1)
                {
                    vertexs.Add(new Vertex(i, new Vector3(values[i][0], values[i][1], values[i][2]), Vector3.UnitY));
                }
                else
                {
                    vertexs.Add(new Vertex(i, new Vector3(values[i][0], values[i][1], values[i][2]), Vector3.UnitX));
                }
            }

            var polygon = new Polygon("perceptron", vertexs);
            RenderObject render = RenderObjectFactory.Instance.CreateRenderObject("perceptron");
            render.SetPolygon(polygon);
            scene.AddObject(render);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
