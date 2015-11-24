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
        event EventHandler Resetted;
        GridSize GridSize { get; }
        int RowCount { get; }
        int ColCount { get;}
        CellInfo GetCellInfo(int row, int col);
        IEnumerable<CellInfo> GetCells();
        void ChangeCell(int row, int col);      
        CellInfo[] Evolve();        
    }

    public class GridCellsChangedEventArgs:EventArgs
    {
        public CellInfo[] Cells { get; set; }
    }

}
