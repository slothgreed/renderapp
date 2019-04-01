using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Gfx.Geometry;
using KI.Asset;
using OpenTK;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// パーセプトロンコマンド
    /// </summary>
    public class PerceptronCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private PerceptronCommandArgs perceptronCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public PerceptronCommand(PerceptronCommandArgs commandArgs)
        {
            perceptronCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult CanExecute()
        {
            return CommandResult.Success;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult Execute()
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
            RenderObject render = RenderObjectFactory.Instance.CreateRenderObject("perceptron", polygon);
            perceptronCommandArgs.Scene.AddObject(render);

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Perceptronのコマンド引数
    /// </summary>
    public class PerceptronCommandArgs
    {
        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetObject">シーン</param>
        public PerceptronCommandArgs(Scene scene)
        {
            Scene = scene;
        }
    }
}
