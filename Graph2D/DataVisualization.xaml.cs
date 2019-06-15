using KI.Analyzer.Parameter;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;

namespace Graph2D.View
{
    /// <summary>
    /// DataVisualization.xaml の相互作用ロジック
    /// </summary>
    public partial class DataVisualization : UserControl
    {
        public DataVisualization()
        {
            InitializeComponent();
        }

        public string GraphName { get; set; } = "Default";

        private ObservableCollection<IParameter> parameterList;

        public ObservableCollection<IParameter> ParameterList
        {
            get
            {
                return parameterList;
            }

            set
            {
                parameterList = value;

                this.ParameterKind.Items.Clear();
                for (int i = 0; i < parameterList.Count; i++)
                {
                    this.ParameterKind.Items.Add(parameterList[i].Name);
                }

                this.ParameterKind.Items.Add("All");
                this.Update(value);
            }
        }

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="name">グラフ名</param>
        /// <param name="parameter">パラメータ</param>
        public void Update(IEnumerable<IParameter> parameters)
        {
            var windowsFormsHost = this.WindowsHost as WindowsFormsHost;
            var chart = (Chart)windowsFormsHost.Child;
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            chart.ChartAreas.Add(GraphName);

            foreach (var paramList in parameters)
            {
                if (paramList is ScalarParameter)
                {
                    Series series = new Series();
                    series.Name = paramList.Name;
                    series.ChartType = SeriesChartType.Line;
                    series.MarkerStyle = MarkerStyle.Circle;

                    var scalarParam = paramList as ScalarParameter;

                    foreach (var param in scalarParam.Values)
                    {
                        series.Points.Add(param);
                    }

                    chart.Series.Add(series);
                    chart.Legends.Add(series.Name);
                }
                else if (paramList is VectorParameter)
                {
                    Series series = new Series();
                    series.Name = paramList.Name;
                    series.ChartType = SeriesChartType.Line;
                    series.MarkerStyle = MarkerStyle.Circle;

                    var vectorParam = paramList as VectorParameter;

                    foreach (var param in vectorParam.Values)
                    {
                        series.Points.AddXY(param.X, param.Y);
                    }

                    chart.Series.Add(series);
                    chart.Legends.Add(series.Name);
                }
            }
        }

        private void ParameterList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var item = comboBox.SelectedItem as string;

            if (item == null)
            {
                return;
            }

            var parameter = ParameterList.Where(p => p.Name == item);

            if (ParameterList.Any(p=>p.Name == item))
            {
                Update(parameter);
            }
            else if (item == "All")
            {
                Update(ParameterList);
            }
        }

        private void TextBox_MaxLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            double value = 0;
            if (double.TryParse(textBox.Text, out value))
            {
                var windowsFormsHost = this.WindowsHost as WindowsFormsHost;
                var chart = (Chart)windowsFormsHost.Child;
                if (chart.ChartAreas.Count != 0)
                {
                    chart.ChartAreas[GraphName].AxisY.Maximum = value;
                }
            }
        }

        private void TextBox_MinLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            double value = 0;
            if (double.TryParse(textBox.Text, out value))
            {
                var windowsFormsHost = this.WindowsHost as WindowsFormsHost;
                var chart = (Chart)windowsFormsHost.Child;
                if (chart.ChartAreas.Count != 0)
                {
                    chart.ChartAreas[GraphName].AxisY.Minimum = value;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var windowsFormsHost = this.WindowsHost as WindowsFormsHost;
            var chart = (Chart)windowsFormsHost.Child;
            if (chart.ChartAreas.Count != 0)
            {
                chart.ChartAreas[GraphName].AxisY.Maximum = double.NaN;
                chart.ChartAreas[GraphName].AxisY.Minimum = double.NaN;

                this.Max.Text = "最大値";
                this.Min.Text = "最小値";
            }
        }
    }
}
