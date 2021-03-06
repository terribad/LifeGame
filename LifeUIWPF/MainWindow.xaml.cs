﻿using System;
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

        GridSettings Settings
        {
            get { return Global.GridSettings; }
            set { Global.GridSettings = value; }
        }

        int CellSize
        {
            get { return Settings.CellSize; }
        }        
        
        DispatcherTimer timer;
        FakeLifeGrid grid;
        ImageBrush imgStart = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Assets/Start-48.png")));
        ImageBrush imgStop = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Assets/Stop-48.png")));
        ImageBrush imgSettings = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Assets/Settings-48.png")));
        ImageBrush imgReset = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Assets/Reset-48.png")));
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateTimer();
            BindImageToButtons();
            CreateGrid();
        }

        void BindImageToButtons()
        {
            btnSettings.Background = imgSettings;
            btnReset.Background = imgReset;
        }

        void CreateGrid()
        {
            grid = new FakeLifeGrid(new GridSize(Settings.RowCount, Settings.ColCount));
            gridView.LifeGrid = grid;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            UpdateUI();
        }
        
        void UpdateGridView(GridSettings settings = null)
        {
            if (settings == null)
                settings = Settings;
            gridView.LiveCellBrush = new SolidColorBrush(settings.LiveCellFillColor);
            gridView.DeadCellBrush = new SolidColorBrush(settings.DeadCellFillColor);
            gridView.ShowGridLines = settings.ShowGridLines;
            gridView.CellSize = settings.CellSize;
        }

        void CreateTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += dispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            grid.Evolve();
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
            grid.Reset();
        }

        private void UpdateUI()
        {
            btnReset.IsEnabled = !timer.IsEnabled;
            btnStart.Background = timer.IsEnabled ? imgStop : imgStart;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            var sd = new SettingsDialog();
            sd.SettingApply += sd_SettingApply;
            sd.GridSettings = Settings;
            var ok = sd.ShowDialog();
            if (ok.HasValue && ok.Value)
            {
                Settings = sd.GridSettings;
                if (grid.RowCount != Settings.RowCount || grid.ColCount != Settings.ColCount)
                    CreateGrid();
            }
            UpdateGridView();
            UpdateUI();
        }

        void sd_SettingApply(object sender, SettingsApplyEventArgs e)
        {
            UpdateGridView(e.GridSettings);
        }
       
    }
}
