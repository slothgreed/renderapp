using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KI.UI.Controls;
using OpenTK;

namespace KI.UI.Controls
{
    /// <summary>
    /// Vector3EditBoxView.xaml の相互作用ロジック
    /// </summary>
    public partial class Vector3EditBox : UserControl
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Vector3EditBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// X 値のプロパティ
        /// </summary>
        public static readonly DependencyProperty XValueProperty =
            DependencyProperty.Register(
                "XValue",
                typeof(float),
                typeof(Vector3EditBox),
                new FrameworkPropertyMetadata(0.0f, new PropertyChangedCallback(OnXValueChanged)));

        /// <summary>
        /// Y 値のプロパティ
        /// </summary>
        public static readonly DependencyProperty YValueProperty =
            DependencyProperty.Register(
                "YValue",
                typeof(float),
                typeof(Vector3EditBox),
                new FrameworkPropertyMetadata(0.0f, new PropertyChangedCallback(OnYValueChanged)));

        /// <summary>
        /// Z 値のプロパティ
        /// </summary>
        public static readonly DependencyProperty ZValueProperty =
            DependencyProperty.Register(
                "ZValue",
                typeof(float),
                typeof(Vector3EditBox),
                new FrameworkPropertyMetadata(
                    0.0f,
                    new PropertyChangedCallback(OnZValueChanged)));

        /// <summary>
        /// Vector3 値のプロパティ
        /// </summary>
        public static readonly DependencyProperty Vector3ValueProperty =
            DependencyProperty.Register(
                "Vector3Value",
                typeof(Vector3),
                typeof(Vector3EditBox),
                new FrameworkPropertyMetadata(
                    Vector3.Zero, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnVectorValueChanged)));

        /// <summary>
        /// X 値のプロパティ
        /// </summary>
        public float XValue
        {
            get { return (float)GetValue(XValueProperty); }
            set { SetValue(XValueProperty, value); }
        }

        /// <summary>
        /// Y 値のプロパティ
        /// </summary>
        public float YValue
        {
            get { return (float)GetValue(YValueProperty); }
            set { SetValue(YValueProperty, value); }
        }

        /// <summary>
        /// Z 値のプロパティ
        /// </summary>
        public float ZValue
        {
            get { return (float)GetValue(ZValueProperty); }
            set { SetValue(ZValueProperty, value); }
        }

        /// <summary>
        /// Vector3 値のプロパティ
        /// </summary>
        public Vector3 Vector3Value
        {
            get { return (Vector3)GetValue(Vector3ValueProperty); }
            set { SetValue(Vector3ValueProperty, value); }
        }

        /// <summary>
        /// X値変更イベント
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnXValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Vector3EditBox property = (Vector3EditBox)d;
            property.OnVectorValueChanged(new Vector3((float)e.NewValue, property.YValue, property.ZValue));
        }

        /// <summary>
        /// Y値変更イベント
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnYValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Vector3EditBox property = (Vector3EditBox)d;
            property.OnVectorValueChanged(new Vector3(property.XValue, (float)e.NewValue, property.ZValue));
        }

        /// <summary>
        /// Z値変更イベント
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnZValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Vector3EditBox property = (Vector3EditBox)d;
            property.OnVectorValueChanged(new Vector3(property.XValue, property.YValue, (float)e.NewValue));
        }

        /// <summary>
        /// Vecotr値変更イベント
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnVectorValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Vector3EditBox property = (Vector3EditBox)d;
            property.OnVectorValueChanged((Vector3)e.NewValue);
        }

        /// <summary>
        /// ベクトル値の変更イベント
        /// </summary>
        /// <param name="newValue">新しい値</param>
        private void OnVectorValueChanged(Vector3 newValue)
        {
            Vector3Value = newValue;
            XValue = newValue.X;
            YValue = newValue.Y;
            ZValue = newValue.Z;
        }
    }
}
