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
using System.Windows.Shapes;

namespace LifeUIWPF
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        public event EventHandler<SettingsApplyEventArgs> SettingApply;

        private GridSettings _gridSettings = new GridSettings();
        public GridSettings GridSettings
        {
            get { return _gridSettings; }
            set { _gridSettings = value; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var settings = GetSettings();
            if (settings.RowCount != GridSettings.RowCount || settings.ColCount != GridSettings.ColCount)
            {
                var cmd = MessageBox.Show("Le dimensioni della griglia sono variate.\nLa griglia sarà creata di nuovo.\nVuoi continuare?", "LifeGame", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (cmd != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            DialogResult = true;
            GridSettings = settings;
            Close();
            
        }

        private GridSettings GetSettings()
        {
            var settings = new GridSettings()
            {
                LiveCellFillColor = cpLiveColor.SelectedColor != null ? cpLiveColor.SelectedColor.Value : Colors.Transparent,
                DeadCellFillColor = cpDeadColor.SelectedColor != null ? cpDeadColor.SelectedColor.Value : Colors.Transparent,
                RowCount = udRows.Value.Value,
                ColCount = udCols.Value.Value,
                CellSize = udCellSize.Value.Value,
                ShowGridLines = chkShowGridLines.IsChecked.GetValueOrDefault()
            };
            return settings;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cpLiveColor.SelectedColor = GridSettings.LiveCellFillColor;
            cpDeadColor.SelectedColor = GridSettings.DeadCellFillColor;
            udCellSize.Value = GridSettings.CellSize;
            udRows.Value = GridSettings.RowCount;
            udCols.Value = GridSettings.ColCount;
            chkShowGridLines.IsChecked = GridSettings.ShowGridLines;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (SettingApply != null)
                SettingApply(this, new SettingsApplyEventArgs() { GridSettings = GetSettings() });
        }

    }

    public class SettingsApplyEventArgs:EventArgs
    {
        public GridSettings GridSettings { get; set; }
    }
}
