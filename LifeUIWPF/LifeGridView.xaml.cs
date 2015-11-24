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
using LifeEngine;
namespace LifeUIWPF
{
    /// <summary>
    /// Interaction logic for LifeGridView.xaml
    /// </summary>
    public partial class LifeGridView : UserControl
    {
        public LifeGridView()
        {
            InitializeComponent();
        }

        const double CELLS_SPACING = 6;        
        static readonly Brush lineBrush = new SolidColorBrush(Colors.LightGray);
        
        Ellipse[,] cellsView;

        private double _cellSize = 36;
        public double CellSize
        {
            get { return _cellSize; }
            private set 
            {
                _cellSize = value;
            }
        }

        private ILifeGrid _lifeGrid;
        public ILifeGrid LifeGrid
        {
            get { return _lifeGrid; }
            set 
            {
                _lifeGrid = value;                
                CreateGrid();
                _lifeGrid.Resetted += _lifeGrid_Resetted;
                _lifeGrid.CellsChanged += _lifeGrid_CellsChanged;
            }
        }

        private Brush _liveCellBrush = new SolidColorBrush(Colors.LightCoral);
        public Brush LiveCellBrush
        {
            get { return _liveCellBrush; }
            set
            {
                _liveCellBrush = value;
                UpdateCellsView();
            }
        }

        private Brush _deadCellBrush;
        public Brush DeadCellBrush
        {
            get { return _deadCellBrush; }
            set
            {
                _deadCellBrush = value;
                UpdateCellsView();
            }
        }

        private bool _showGridLines = true;
        public bool ShowGridLines
        {
            get { return _showGridLines; }
            set 
            { 
                _showGridLines = value;
                UpdateLinesView();
            }
        }

        private int RowCount { get { return LifeGrid.RowCount; } }
        private int ColCount { get { return LifeGrid.ColCount; } }

        void el_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeCell(sender as Ellipse);
        }
        void _lifeGrid_CellsChanged(object sender, GridCellsChangedEventArgs e)
        {
            UpdateCellsChangedView(e.Cells);
        }

        void _lifeGrid_Resetted(object sender, EventArgs e)
        {
            UpdateCellsView();
        }
        
        private void ChangeCell(Ellipse cellView)
        {
            int row, col;
            GetCellViewPosition(cellView, out row, out col);
            LifeGrid.ChangeCell(row, col);
        }

        private void GetCellViewPosition(Ellipse cellView, out int row, out int col)
        {
            for (row = 0; row < RowCount; row++)
            {
                for (col = 0; col < ColCount; col++)
                {
                    if (cellsView[row,col] == cellView)
                        return;
                }
            }
            row = -1;
            col = -1;
        }

        private void CreateGrid()
        {
            canvas.Children.Clear();
            CellSize = canvas.ActualWidth / ColCount;
            //canvas.Width = CellSize * ColCount;
            //canvas.Height = CellSize * RowCount;
            cellsView = new Ellipse[RowCount, ColCount];
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColCount; c++)
                {
                    var cell = CreateCell(r, c);
                    cellsView[r, c] = cell;
                    UpdateCellView(r, c, false);
                }
            }
            CreateGridLines();
        }

        private Ellipse CreateCell(int r, int c)
        {
            Ellipse el = new Ellipse();
            el.Width = CellSize - CELLS_SPACING;
            el.Height = CellSize - CELLS_SPACING;
            el.StrokeThickness = 1;
            Canvas.SetLeft(el, c * CellSize + CELLS_SPACING / 2);
            Canvas.SetTop(el, r * CellSize + CELLS_SPACING / 2);
            el.MouseDown += el_MouseDown;
            el.Tag = false;
            canvas.Children.Add(el);
            return el;
        }

        private void CreateGridLines()
        {
            for (int r = 1; r < RowCount; r++)
            {
                CreateHorizontalLine(r);
            }

            for (int c = 1; c < ColCount; c++)
            {
                CreateVerticalLine(c);
            }
        }

        private void CreateVerticalLine(int c)
        {
            Line li = new Line();
            li.X1 = c * CellSize;
            li.X2 = li.X1;
            li.Y1 = 0;
            li.Y2 = canvas.ActualHeight;
            li.Stroke = lineBrush;
            canvas.Children.Add(li);
        }

        private void CreateHorizontalLine(int r)
        {
            Line li = new Line();
            li.X1 = 0;
            li.X2 = canvas.ActualWidth;
            li.Y1 = r * CellSize;
            li.Y2 = li.Y1;
            li.Stroke = lineBrush;
            canvas.Children.Add(li);
        }

        #region UPDATE CELL VIEW
        private void UpdateCellsChangedView(CellInfo[] cellsChanged)
        {
            foreach (var ci in cellsChanged)
                UpdateCellView(ci.Row, ci.Col, ci.Live);
        }

        private void UpdateCellsView()
        {
            foreach (var ci in LifeGrid.GetCells())
                UpdateCellView(ci.Row, ci.Col, ci.Live);
        }

        private void UpdateCellView(int row, int col, bool live)
        {
            var cv = cellsView[row, col];
            UpdateCellView(cv, live);
        }

        private void UpdateCellView(Ellipse cellView, bool live)
        {
            cellView.Fill = (live) ? LiveCellBrush : DeadCellBrush;
        }
        #endregion  

        private void UpdateLinesView()
        {
            foreach (var line in canvas.Children.OfType<Line>())
            {
                UpdateLineView(line);
            }
        }

        private void UpdateCellView(Ellipse cell)
        {
            if ((bool)cell.Tag)
                cell.Fill = LiveCellBrush;
            else
                cell.Fill = DeadCellBrush;
        }

        private void UpdateLineView(Line line)
        {
            if (ShowGridLines)
                line.Stroke = lineBrush;
            else
                line.Stroke = Background;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateGrid();
            DeadCellBrush = Background;
        }
    }
}
