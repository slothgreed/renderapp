using System.Windows;
using System.Windows.Controls;
using OpenTK;

namespace KI.Presentation.Controls
{
    /// <summary>
    /// Vector3EditBoxView.xaml の相互作用ロジック
    /// </summary>
    public partial class Vector3EditBoxView : UserControl
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Vector3EditBoxView()
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
                typeof(Vector3EditBoxView),
                new FrameworkPropertyMetadata(0.0f, new PropertyChangedCallback(OnXValueChanged)));

        /// <summary>
        /// Y 値のプロパティ
        /// </summary>
        public static readonly DependencyProperty YValueProperty =
            DependencyProperty.Register(
                "YValue",
                typeof(float),
                typeof(Vector3EditBoxView),
                new FrameworkPropertyMetadata(0.0f, new PropertyChangedCallback(OnYValueChanged)));

        /// <summary>
        /// Z 値のプロパティ
        /// </summary>
        public static readonly DependencyProperty ZValueProperty =
            DependencyProperty.Register(
                "ZValue",
                typeof(float),
                typeof(Vector3EditBoxView),
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
                typeof(Vector3EditBoxView),
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
            Vector3EditBoxView property = (Vector3EditBoxView)d;
            property.OnVectorValueChanged(new Vector3((float)e.NewValue, property.YValue, property.ZValue));
        }

        /// <summary>
        /// Y値変更イベント
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnYValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Vector3EditBoxView property = (Vector3EditBoxView)d;
            property.OnVectorValueChanged(new Vector3(property.XValue, (float)e.NewValue, property.ZValue));
        }

        /// <summary>
        /// Z値変更イベント
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnZValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Vector3EditBoxView property = (Vector3EditBoxView)d;
            property.OnVectorValueChanged(new Vector3(property.XValue, property.YValue, (float)e.NewValue));
        }

        /// <summary>
        /// Vecotr値変更イベント
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnVectorValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Vector3EditBoxView property = (Vector3EditBoxView)d;
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
