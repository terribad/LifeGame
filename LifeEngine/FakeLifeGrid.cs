using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeEngine
{
    public class FakeLifeGrid: ILifeGrid
    {
        public event EventHandler<GridCellsChangedEventArgs> CellsChanged;
        public event EventHandler Resetted;

        Random rnd = new Random();
        private bool[,] grid;

        public FakeLifeGrid(GridSize size)
        {
            this.GridSize = size;
            grid = new bool[RowCount, ColCount];
        }

        public GridSize GridSize { get; private set; }
        public int RowCount { get { return GridSize.RowCount; } }
        public int ColCount { get { return GridSize.ColCount; } }

        public void Reset()
        {
            grid = new bool[RowCount, ColCount];
            OnResetted();
        }

        public CellInfo GetCellInfo(int row, int col)
        {
            return new CellInfo { Row = row, Col = col, Live = grid[row, col] };
        }

        public void ChangeCell(int row, int col)
        {
            grid[row, col] = !grid[row, col];
            var cells = new CellInfo[] { GetCellInfo(row, col)};
            OnCellsChanged(new GridCellsChangedEventArgs { Cells = cells});
        }

        public IEnumerable<CellInfo> GetCells()
        {
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    yield return new CellInfo { Row = r, Col = c, Live = grid[r, c] };
                }
            }
        }
        public CellInfo[] Evolve()
        {
            var nc = rnd.Next(1, 8);
            var cells = new CellInfo[nc];
            for (int i = 0; i < nc; i++)
            {
                int r = rnd.Next(0, RowCount);
                int c = rnd.Next(0, ColCount);
                grid[r,c] = !grid[r,c];
                var cell = new CellInfo { Row = r, Col = c, Live = grid[r,c]};
                cells[i] = cell;
            }
            OnCellsChanged(new GridCellsChangedEventArgs { Cells=cells});
            return cells;
        }

        private void OnCellsChanged(GridCellsChangedEventArgs e)
        {
            if (CellsChanged!= null)
                CellsChanged(this, e);
        }

        private void OnResetted()
        {
            if (Resetted != null)
                Resetted(this, EventArgs.Empty);
        }



        
    }

    
}
