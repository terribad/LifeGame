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

using LifeEngine;
using Xceed.Wpf.Toolkit;
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

        const int GRID_COUNT = 15;
        const double CELL_SIZE = 46;
        
        DispatcherTimer timer;
        FakeLifeGrid grid;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateGrid();
            CreateTimer();
            cpLiveColor.SelectedColorChanged += colorPicker_SelectedColorChanged;
            cpDeadColor.SelectedColorChanged += colorPicker_SelectedColorChanged;
            ResizeWindowOnGridSize();
            gridView.GridSize = grid.GridSize;            
            UpdateUI();
        }

        void CreateGrid()
        {
            grid = new FakeLifeGrid(new GridSize(GRID_COUNT, GRID_COUNT));
        }
        
        void ResizeWindowOnGridSize()
        {
            Width = CELL_SIZE * grid.ColCount + gridView.Margin.Left + gridView.Margin.Right;
            Height = CELL_SIZE * grid.RowCount + gridView.Margin.Top + gridView.Margin.Bottom;
        }

        void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ColorPicker cp = sender as ColorPicker;
            if (!cp.SelectedColor.HasValue)
                return;
            if (cp.Name == "cpLiveColor")
                gridView.LiveCellBrush = new SolidColorBrush(cpLiveColor.SelectedColor.Value);
            else
                gridView.DeadCellBrush = new SolidColorBrush(cpDeadColor.SelectedColor.Value);
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
            btnShowGrid.Content = gridView.ShowGridLines ? "Nascondi griglia" : "Mostra griglia";
        }

        private void btnShowGrid_Click(object sender, RoutedEventArgs e)
        {
            gridView.ShowGridLines = !gridView.ShowGridLines;
        }
       
    }
}
