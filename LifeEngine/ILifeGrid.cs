using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeEngine
{
    public interface ILifeGrid
    {
        event EventHandler<GridCellsChangedEventArgs> CellsChanged;
        GridSize GridSize { get; }
        int RowCount { get; }
        int ColCount { get;}
        IEnumerable<CellInfo> GetCells();
        CellInfo[] Evolve();        
    }

    public class GridCellsChangedEventArgs:EventArgs
    {
        public CellInfo[] Cells { get; set; }
    }

}
