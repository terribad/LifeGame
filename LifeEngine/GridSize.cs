using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeEngine
{
    public class GridSize
    {
        public GridSize(int rowCount, int colCount)
        {
            this.RowCount = rowCount;
            this.ColCount = colCount;
        }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
    }
}
