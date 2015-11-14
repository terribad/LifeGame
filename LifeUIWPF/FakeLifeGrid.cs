﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeUIWPF
{
    public class FakeLifeGrid
    {
        Random rnd = new Random();
        private bool[,] grid;

        public FakeLifeGrid(int rowCount, int colCount)
        {
            this.GridSize = new GridSize(rowCount, colCount);
            grid = new bool[RowCount, ColCount];
        }

        public GridSize GridSize { get; private set; }
        public int RowCount { get { return GridSize.RowCount; } }
        public int ColCount { get { return GridSize.ColCount; } }
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
            return cells;
        }
    }

    public class CellInfo
    {
        public int Row;
        public int Col;
        public bool Live;
    }

    public class GridSize
    {
        public GridSize (int rowCount, int colCount)
        {
            this.RowCount = rowCount;
            this.ColCount = colCount;
        }
        public int RowCount {get;private set;}
        public int ColCount { get; private set; }
    }
}
