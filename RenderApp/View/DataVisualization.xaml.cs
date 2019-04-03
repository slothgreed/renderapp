using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;
using KI.Analyzer.Parameter;

namespace RenderApp.View
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

        public string GraphName { get; set; }

        private Dictionary<string, IParameter> parameterList;

        public Dictionary<string, IParameter> ParameterList
        {
            get
            {
                return parameterList;
            }

            set
            {
                parameterList = value;

                this.ParameterKind.Items.Clear();
                foreach (var paramName in value.Keys)
                {
                    this.ParameterKind.Items.Add(paramName);
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
        public void Update(Dictionary<string, IParameter> parameters)
        {
            var windowsFormsHost = this.WindowsHost as WindowsFormsHost;
            var chart = (Chart)windowsFormsHost.Child;
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            chart.ChartAreas.Add(GraphName);
            //chart.ChartAreas[GraphName].AxisY.Maximum = 2;
            //chart.ChartAreas[GraphName].AxisY.Minimum = -2;

            foreach (var paramList in parameters)
            {
                if (paramList.Value is ScalarParameter)
                {
                    Series series = new Series();
                    series.Name = paramList.Key;
                    series.ChartType = SeriesChartType.Line;
                    series.MarkerStyle = MarkerStyle.Circle;
                   
                    var scalarParam = paramList.Value as ScalarParameter;

                    foreach (var param in scalarParam.Values)
                    {
                        series.Points.Add(param);
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
            else if (ParameterList.ContainsKey(item))
            {
                Update(new Dictionary<string, IParameter>() { { item, ParameterList[item] } });
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
                    chart.ChartAreas[GraphName].AxisY.Minimum = value;
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
                    chart.ChartAreas[GraphName].AxisY.Maximum = value;
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

                this.Max.Text = "0";
                this.Min.Text = "0";
            }

        }
    }
}
