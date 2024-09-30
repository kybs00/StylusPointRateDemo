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

        private int _moveEntryCount = 0;
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
            _moveEntryCount++;
            var currentPointsCount = e.GetStylusPoints(this).Count;
            _pointsCount += currentPointsCount;
            _distinctPoints.AddRange(e.GetStylusPoints(this).Distinct());
        }

        private void MainWindow_OnStylusMove(object sender, StylusEventArgs e)
        {
            if (!_stylusDown)
            {
                return;
            }
            _moveEntryCount++;
            var currentPointsCount = e.GetStylusPoints(this).Count;
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
                                  $"输入平均间隔{Math.Round((Environment.TickCount - _startTick) / (double)_moveEntryCount, 2)}ms,\r\n" +
                                  $"输入单次数量在({_minMoveCount},{_maxMoveCount})区间\r\n" +
                                  $"点平均间隔{Math.Round((Environment.TickCount - _startTick) / (double)_pointsCount, 2)}ms\r\n");
            _pointsCount = 0;
            _moveEntryCount = 0;
            _distinctPoints.Clear();
            _stylusDown = false;
        }
    }
}