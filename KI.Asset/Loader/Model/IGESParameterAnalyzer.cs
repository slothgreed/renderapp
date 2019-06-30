using KI.Foundation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Loader.Model
{
    enum IGESPrimitiveType
    {
        Circle = 100,
        CompositeCurve = 102,
        ConincArc = 104,
        Plane = 108,
        Line = 110,
        SplineCurve = 112,
        SplineSurface = 114,
        Point = 116,
        RuledSurface = 118,
        BSplineCurve = 126,
        BSplineSurface = 128,
        OffsetCurve = 130,
        OffsetSurface = 140,
        CurveOnAParametericSurface = 142,
        TrimmedSurface = 144,
        Torus = 160,
        Boundary = 141,
        BoundarySurface = 143,
        ColorDefinition = 314
    }

    /// <summary>
    /// IGESファイルのパラメータセクションの解析を行う。
    /// </summary>
    public class IGESParameterAnalyzer
    {
        public IGESParameterAnalyzer()
        {
        }


        /// <summary>
        /// 形状種類の数(デバッグ用)
        /// </summary>
        private Dictionary<IGESPrimitiveType, int> primitiveTypeNum = new Dictionary<IGESPrimitiveType, int>();

        /// <summary>
        /// a,b,c,d・・・,n;形式の文字列を解析する
        /// (72行目以降の情報を切り捨てて結合したもの)
        /// </summary>
        /// <param name="parameterStr">パラメータセクションの文字列</param>
        /// <param name="delimiter">デリミタ</param>
        /// <returns>成功したかどうか</returns>
        public bool Analyze(string parameterStr, char delimiter)
        {
            var splitData = parameterStr.Split(delimiter);
            if (Enum.IsDefined(typeof(IGESPrimitiveType), splitData[0]))
            {
                return false;
            }

            IGESPrimitiveType parameterType = (IGESPrimitiveType)Enum.Parse(typeof(IGESPrimitiveType), splitData[0]);

            if (primitiveTypeNum.ContainsKey(parameterType))
            {
                primitiveTypeNum[parameterType]++;
            }
            else
            {
                primitiveTypeNum.Add(parameterType, 1);
            }

            switch (parameterType)
            {
                case IGESPrimitiveType.Circle:
                    break;
                case IGESPrimitiveType.CompositeCurve:
                    break;
                case IGESPrimitiveType.ConincArc:
                    break;
                case IGESPrimitiveType.Plane:
                    break;
                case IGESPrimitiveType.Line:
                    break;
                case IGESPrimitiveType.SplineCurve:
                    break;
                case IGESPrimitiveType.SplineSurface:
                    break;
                case IGESPrimitiveType.Point:
                    break;
                case IGESPrimitiveType.RuledSurface:
                    break;
                case IGESPrimitiveType.BSplineCurve:
                    break;
                case IGESPrimitiveType.BSplineSurface:
                    break;
                case IGESPrimitiveType.OffsetCurve:
                    break;
                case IGESPrimitiveType.OffsetSurface:
                    break;
                case IGESPrimitiveType.Torus:
                    break;
                default:
                    return true;
            }

            return true;
        }

        public void ParameterDataLog()
        {
            foreach (KeyValuePair<IGESPrimitiveType, int> info in primitiveTypeNum)
            {
                Logger.Log(Logger.LogLevel.Allway, "PrimitiveType :" + info.Key + ", Num :" + info.Value);
            }

            Logger.Log(Logger.LogLevel.Allway, "-------------------------------------------------------");
        }
    }
}
