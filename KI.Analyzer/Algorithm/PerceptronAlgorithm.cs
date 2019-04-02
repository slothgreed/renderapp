using System.Linq;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// パーセプトロンアルゴリズム
    /// </summary>
    public class PerceptronAlgorithm
    {
        /// <summary>
        /// 学習係数
        /// </summary>
        private float eta;

        /// <summary>
        /// 繰り返し回数
        /// </summary>
        private int loopNum;

        /// <summary>
        /// データ
        /// </summary>
        private float[][] inputData;

        /// <summary>
        /// 結果
        /// </summary>
        private float[] results;

        /// <summary>
        /// ステップに応じたエラーの数
        /// </summary>
        private int[] errorNum;

        /// <summary>
        /// 重み
        /// </summary>
        private float[] weights;
        
        /// <summary>
        /// 重み
        /// </summary>
        public float[] Weight
        {
            get
            {
                return weights;
            }
        }

        /// <summary>
        /// Constractor.
        /// </summary>
        /// <param name="eta">学習係数</param>
        /// <param name="loop">繰り返し回数</param>
        /// <param name="datas">データ</param>
        /// <param name="results">結果</param>
        public PerceptronAlgorithm(float eta, int loop, float[][] datas, float[] results)
        {
            this.eta = eta;
            this.loopNum = loop;
            this.inputData = datas;
            this.results = results;
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        public void Calculate()
        {
            weights = new float[inputData[0].Length];
            errorNum = new int[inputData.Length];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = 0;
            }

            for (int i = 0; i < loopNum; i++)
            {
                int errors = 0;
                for (int j = 0; j < inputData.Length; j++)
                {
                    float updateWeight = eta * (results[i] - Predict(inputData[i],weights));
                    for (int k = 0; k < weights.Length; k++)
                    {
                        weights[k] = updateWeight * inputData[j][k];
                    }

                    if ((int)updateWeight != 0)
                    {
                        errors++;
                    }
                }

                errorNum[i] = errors;
            }
        }

        /// <summary>
        /// パーセプトロンの入力計算
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns>合計値</returns>
        private float[] Sum(float[] data, float[] weight)
        {
            var value = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                value[i] += weight[i] * data[i];
            }

            return value;
        }

        /// <summary>
        /// ヘビサイド関数の適用
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>結果</returns>
        private int Heaviside(float[] value)
        {
            if (value.Any(p => p > 0))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 予測
        /// </summary>
        /// <param name="inputData">データ</param>
        /// <param name="weights">重み</param>
        /// <returns>結果</returns>
        public int Predict(float[] inputData,float[] weights)
        {
            return Heaviside(Sum(inputData, weights));
        }
    }
}
