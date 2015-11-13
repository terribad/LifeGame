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
using System.Windows.Threading;

namespace LifeUIWPF
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

        DispatcherTimer timer;
        FakeLifeGrid grid = new FakeLifeGrid(10,10);
        
        private void content_Loaded(object sender, RoutedEventArgs e)
        {
            

            CreateTimer();
            UpdateUI();
        }

        void Start()
        {
            timer.Start();
        }

        void Stop()
        {
            timer.Stop();
        }
        
        void CreateTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += dispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            CellInfo[] cellsChanged = grid.Evolve();
            gridView.ChangeCells(cellsChanged);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();
            else
                timer.Start();
            UpdateUI();            
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            gridView.Reset();
        }

        private void UpdateUI()
        {
            btnReset.IsEnabled = !timer.IsEnabled;
            btnStart.Content = timer.IsEnabled ? "Stop" : "Start";
        }
    }
}
