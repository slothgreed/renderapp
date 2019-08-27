using Graph2D.View;
using KI.Analyzer.Parameter;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graph2D
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var sin_value = new float[360];
            var cos_value = new float[360];
            var tan_value = new float[360];
            for (int i = 0; i < sin_value.Length; i++)
            {
                sin_value[i] = (float)Math.Cos(i / 10.0f);
                cos_value[i] = (float)Math.Sin(i / 10.0f);
                tan_value[i] = (float)Math.Tan(i / 10.0f);
            }

            //AddParameter("Cos", sin_value);
            //AddParameter("Sin", cos_value);
            //AddParameter("Tan", tan_value);
            //BspCalc();
            //AddParameter("Spline", vector);
            //AddParameter("Control", controlPoint);
            AddParameter("Control", controlPoint);
            AddParameter("Spline", Evaluate());
        }

        public void AddParameter(string key, IEnumerable<float> values)
        {
            if(Graph2D.ParameterList == null)
            {
                Graph2D.ParameterList = new ObservableCollection<IParameter>();
            }

            ScalarParameter parameter = new ScalarParameter(key, values.ToArray());
            Graph2D.ParameterList.Add(parameter);
            Graph2D.ParameterList = Graph2D.ParameterList;
        }

        public void AddParameter(string key, IEnumerable<Vector3> values)
        {
            if (Graph2D.ParameterList == null)
            {
                Graph2D.ParameterList = new ObservableCollection<IParameter>();
            }

            VectorParameter parameter = new VectorParameter(key, values.ToArray());
            Graph2D.ParameterList.Add(parameter);
            Graph2D.ParameterList = Graph2D.ParameterList;
        }

        //List<Vector3> vector = new List<Vector3>();
        //Vector3[] controlPoint =
        //{
        //    new Vector3(30,200,0) ,
        //    new Vector3(60,80,0) ,
        //    new Vector3(150,50,0),
        //    new Vector3(200,220,0),
        //    new Vector3(250,50,0)
        //};
        ////int tmax;
        ////float[] nv = { 0, 1, 2, 3, 4, 5, 6, 7 };
        //float[] nv = { 0, 0, 0, 1, 2, 3, 3, 3 };
        ////float[] nv = { 1, 2, 3, 4, 5, 6, 7, 8 };

        //private float baseN(int i, int k, float t, float[] nv)
        //{
        //    float w1 = 0.0f, w2 = 0.0f;
        //    if (k == 1)
        //    {
        //        if (t >= nv[i] && t < nv[i + 1])
        //            return 1.0f;
        //        else
        //            return 0.0f;
        //    }
        //    else
        //    {
        //        if ((nv[i + k] - nv[i + 1]) != 0)
        //        {
        //            w1 = ((nv[i + k] - t) / (nv[i + k] - nv[i + 1])) * baseN(i + 1, k - 1, t, nv);
        //        }
        //        if ((nv[i + k - 1] - nv[i]) != 0)
        //        {
        //            w2 = ((t - nv[i]) / (nv[i + k - 1] - nv[i])) * baseN(i, k - 1, t, nv);
        //        }
        //        return (w1 + w2);
        //    }
        //}
        //private void BspCalc()
        //{
        //    float [] nv = {0,0,0,1,2,3,3,3};
        //    //float[] nv = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        //    //for (int i = 0; i < 8; i++)
        //    //{
        //    //    nv[i] = nv[i] / 8.0f;
        //    //}
        //    int tmax = 3;
        //    //int [] nv = { 1, 2, 3, 4, 5, 6, 7, 8 };
        //    //int tmax =7;
        //    int j;

        //    vector.Clear();
        //    for (j = 0; j <= tmax * 10; j++)
        //    {
        //        float t = (float)j / 10.0f;
        //        if (j == tmax * 10) t -= 0.001f;//t>=tmax では基底関数は0になる
        //        float x = 0.0f, y = 0.0f;
        //        for (int i = 0; i < controlPoint.Length; i++)
        //        {
        //            float r = baseN(i, 3, t, nv);
        //            x += px[i] * r;
        //            y += py[i] * r;
        //        }

        //        vector.Add(new Vector3(x, y, 0));
        //    }

        //    vector.Clear();
        //    for (float p = 0; p < 3; p += 0.01f)
        //    {
        //        float x = 0.0f, y = 0.0f;
        //        for (int i = 0; i < controlPoint.Length; i++)
        //        {
        //            float r = baseN(i, 3, p, nv);
        //            x += px[i] * r;
        //            y += py[i] * r;
        //        }

        //        vector.Add(new Vector3(x, y, 0));
        //    }

        //}


        Vector3[] controlPoint =
        {
            new Vector3(30,200,0) ,
            new Vector3(60,80,0) ,
            new Vector3(150,50,0),
            new Vector3(200,220,0),
            new Vector3(250,50,0)
        };

        private float BSplineBasisFunc(int i, int degree, float t, float[] knots)
        {
            if (degree == 0)
            {
                if (t >= knots[i] && t < knots[i + 1])
                    return 1f;
                else
                    return 0f;
            }

            float w1 = 0f;
            float w2 = 0f;
            float denominatorA = knots[i + degree] - knots[i];
            float denominatorB = knots[i + degree + 1] - knots[i + 1];

            if (denominatorA != 0f)
                w1 = (t - knots[i]) / denominatorA;

            if (denominatorB != 0f)
                w2 = (knots[i + degree + 1] - t) / denominatorB;


            float firstTerm = 0f;
            float secondTerm = 0f;

            if (w1 != 0f)
                firstTerm = w1 * BSplineBasisFunc(i, degree - 1, t, knots);

            if (w2 != 0f)
                secondTerm = w2 * BSplineBasisFunc(i + 1, degree - 1, t, knots);

            return firstTerm + secondTerm;
        }

        //float[] nv = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        float[] nv = { 0, 0, 0, 0, 0.5f, 1, 1, 1, 1 };

        /// <summary>
        /// 線を引く点を評価し返す。
        /// </summary>
        public Vector3[] Evaluate()
        {
            int degree = 3;
            // 分割数のチェック
            int divideCount = 100;


            Vector3[] linePoints;
            linePoints = new Vector3[divideCount + 1];

            float lowKnot = this.nv[degree];
            float highKnot = this.nv[this.controlPoint.Length];
            float val = highKnot - lowKnot;
            if (val == 0.0f) { val = 1f; }
            float step = val / divideCount;

            for (int p = 0; p <= divideCount; ++p)
            {
                float t = p * step + lowKnot;
                if (t >= highKnot) t = highKnot - 0.000001f;

                linePoints[p] = this.CalcuratePoint(t);
            }

            return linePoints;
        }

        /// <summary>
        /// 指定の位置の座標を取得。
        /// </summary>
        protected virtual Vector3 CalcuratePoint(float t)
        {
            Vector3 linePoint = Vector3.Zero;

            for (int i = 0; i < this.controlPoint.Length; ++i)
            {
                float bs = BSplineBasisFunc(i, 3, t, nv);
                linePoint += bs * this.controlPoint[i];
            }

            return linePoint;
        }

    }
}
