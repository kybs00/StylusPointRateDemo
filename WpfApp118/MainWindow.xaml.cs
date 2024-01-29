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

        private void MainWindow_OnStylusDown(object sender, StylusDownEventArgs e)
        {
            _startTick = Environment.TickCount;
            StylusMove += MainWindow_StylusMove;
        }

        private void MainWindow_StylusMove(object sender, StylusEventArgs e)
        {
            _moveEntryCount++;
            _pointsCount += e.GetStylusPoints(this).Count;
            foreach (var stylusPoint in e.GetStylusPoints(this).Distinct())
            {
                _distinctPoints.Add(stylusPoint);
            }
        }

        private void MainWindow_OnStylusUp(object sender, StylusEventArgs e)
        {
            StylusMove -= MainWindow_StylusMove;
            if (_pointsCount == 0)
            {
                return;
            }
            MessageBox.Show(this, $"一共{_pointsCount}点,非重复{_distinctPoints.Count}点，\r\n" +
                                  $"平均输入间隔{(Environment.TickCount - _startTick) / _moveEntryCount}ms\r\n" +
                                  $"平均点间隔{(Environment.TickCount - _startTick) / _pointsCount}ms\r\n");
            _pointsCount = 0;
            _moveEntryCount = 0;
            _distinctPoints.Clear();
        }
    }
}