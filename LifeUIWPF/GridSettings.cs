using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LifeUIWPF
{
    public class GridSettings
    {
        public GridSettings()
        {
            LiveCellFillColor = Colors.DarkSalmon;
            DeadCellFillColor = Colors.Transparent;
            RowCount = 20;
            ColCount = 20;
            CellSize = 36;
            ShowGridLines = true;
        }
        public Color LiveCellFillColor { get; set; }
        public Color DeadCellFillColor { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public int CellSize { get; set; }
        public bool ShowGridLines { get; set; }
    }
}
