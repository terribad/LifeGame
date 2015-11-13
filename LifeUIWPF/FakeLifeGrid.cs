using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeUIWPF
{
    public class FakeLifeGrid
    {
        Random rnd = new Random();

        public FakeLifeGrid(int rowCount, int colCount)
        {
            this.RowCount = rowCount;
            this.ColCount = colCount;
        }

        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public int[,] GetCellChanged()
        {
            var nc = rnd.Next(1, 8);
            var cells = new int[nc, 2];
            for (int i = 0; i < nc; i++)
            {
                cells[i, 0] = rnd.Next(0, RowCount);
                cells[i, 1] = rnd.Next(0, ColCount);
            }
            return cells;
        }
    }
}
