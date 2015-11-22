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
        
        Ellipse[,] cells;

        public Size CellSize { get; private set; }        

        public void SetCellSize(int value)
        {
            CellSize = new Size(value, value);
            UpdateView();
        }

        private GridSize _gridSize = new GridSize(10,10);
        public GridSize GridSize
        {
            get { return _gridSize; }
            set
            {
                _gridSize = value;
                CreateGrid();
            }
        }

        private Brush _liveCellBrush = new SolidColorBrush(Colors.LightCoral);
        public Brush LiveCellBrush
        {
            get { return _liveCellBrush; }
            set
            {
                _liveCellBrush = value;
                UpdateView();
            }
        }

        private Brush _deadCellBrush;
        public Brush DeadCellBrush
        {
            get { return _deadCellBrush; }
            set
            {
                _deadCellBrush = value;
                UpdateView();
            }
        }

        private bool _showGridLines = true;

        public bool ShowGridLines
        {
            get { return _showGridLines; }
            set 
            { 
                _showGridLines = value;
                UpdateView();
            }
        }

        public int RowCount { get { return GridSize.RowCount; } }
        public int ColCount { get { return GridSize.ColCount; } }

        public void Reset()
        {
            foreach (var cell in cells)
	        {
                SetCell(cell, false);
	        } 
        }

        public void ChangeCells(CellInfo[] cellsChanged)
        {
            foreach (var cell in cellsChanged)
            {
                SetCell(cell.Row, cell.Col, cell.Live);
            }
        }
        private Ellipse CreateCell(int r, int c)
        {
            Ellipse el = new Ellipse();
            el.Width = CellSize.Width - CELLS_SPACING;
            el.Height = CellSize.Height - CELLS_SPACING;
            el.StrokeThickness = 1;
            Canvas.SetLeft(el, c * (CellSize.Width) + CELLS_SPACING/2);
            Canvas.SetTop(el, r * (CellSize.Height) + CELLS_SPACING/2);
            el.MouseDown += el_MouseDown;
            el.Tag = false;
            canvas.Children.Add(el);
            return el;
        }

        void el_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse cell = sender as Ellipse;
            ChangeCell(cell);
        }

        private void SetCell(int row, int col, bool live)
        {
            SetCell(cells[row, col], live);
        }

        private void SetCell(Ellipse cell, bool live)
        {
            cell.Tag = live;
            UpdateCellView(cell);
        }
        
        private void ChangeCell(Ellipse cell)
        {
            bool live = !(bool)cell.Tag;
            SetCell(cell, live);            
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
            li.X1 = c * CellSize.Width;
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
            li.Y1 = r * CellSize.Height;
            li.Y2 = li.Y1;
            li.Stroke = lineBrush;
            canvas.Children.Add(li);
        }

        private void CreateGrid()
        {
            canvas.Children.Clear();
            CellSize = new Size(canvas.ActualWidth / ColCount, canvas.ActualHeight / RowCount);
            cells = new Ellipse[RowCount, ColCount];
            CreateGridLines();
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColCount; c++)
                {
                    var cell = CreateCell(r, c);
                    SetCell(cell, false);
                    cells[r, c] = cell;
                }
            }
        }

        private void UpdateView()
        {
            foreach (var cell in cells)
            {
                UpdateCellView(cell);
            }
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
            cell.Width = CellSize.Width - CELLS_SPACING;
            cell.Height = CellSize.Height - CELLS_SPACING;
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
