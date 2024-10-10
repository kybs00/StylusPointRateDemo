using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp118
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private int _stylusEntryCount = 0;
        private int _pointsCount = 0;
        private List<StylusPoint> _distinctPoints = new List<StylusPoint>();
        private int _startTick = 0;
        private int _minMoveCount = int.MaxValue;
        private int _maxMoveCount = int.MinValue;

        private bool _stylusDown = false;
        private void MainWindow_OnStylusDown(object sender, StylusDownEventArgs e)
        {
            _stylusDown = true;
            _startTick = Environment.TickCount;
            _stylusEntryCount++;
            var currentPointsCount = e.GetStylusPoints(this).Distinct().Count();
            _pointsCount += currentPointsCount;
            _distinctPoints.AddRange(e.GetStylusPoints(this).Distinct());
        }

        private void MainWindow_OnStylusMove(object sender, StylusEventArgs e)
        {
            if (!_stylusDown)
            {
                return;
            }
            var stylusPointCollection = e.GetStylusPoints(this);
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}:MainWindow_OnStylusMove {stylusPointCollection.Count}个点");
            _stylusEntryCount++;
            var currentPointsCount = e.GetStylusPoints(this).Distinct().Count();
            _minMoveCount = Math.Min(_minMoveCount, currentPointsCount);
            _maxMoveCount = Math.Max(_maxMoveCount, currentPointsCount);
            _pointsCount += currentPointsCount;
            _distinctPoints.AddRange(e.GetStylusPoints(this).Distinct());
        }

        private void MainWindow_OnStylusUp(object sender, StylusEventArgs e)
        {
            if (_pointsCount == 0)
            {
                return;
            }
            MessageBox.Show(this, $"一共{_pointsCount}点,非重复{_distinctPoints.Count}点，\r\n" +
                                  $"输入平均间隔{Math.Round((Environment.TickCount - _startTick) / (double)_stylusEntryCount, 2)}ms,\r\n" +
                                  $"单次输入包含({_minMoveCount},{_maxMoveCount})个点\r\n" +
                                  $"点平均间隔{Math.Round((Environment.TickCount - _startTick) / (double)_pointsCount, 2)}ms\r\n");
            _pointsCount = 0;
            _stylusEntryCount = 0;
            _distinctPoints.Clear();
            _stylusDown = false;
        }
    }
}