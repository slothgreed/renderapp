using OpenTK;

namespace KI.Analyzer.Parameter
{
    /// <summary>
    /// ベクトルパラメータ
    /// </summary>
    public class VectorParameter : IParameter
    {
        /// <summary>
        /// 値
        /// </summary>
        public Vector3[] Values { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="val">値</param>
        public VectorParameter(string name, Vector3[] val)
        {
            Name = name;
            Values = val;
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 値の取得
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <returns>値</returns>
        public Vector3 GetValue(int index)
        {
            return Values[index];
        }
    }
}
