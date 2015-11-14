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
        static readonly SolidColorBrush liveBrush = new SolidColorBrush(Colors.LightCoral);
        static readonly SolidColorBrush deadGrayCellsBrush = new SolidColorBrush(Colors.LightGray);
        Brush deadBrush = deadGrayCellsBrush;

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

        private GridViewType _gridViewType = GridViewType.GridLines;
        public GridViewType GridViewType
        {
            get { return _gridViewType; }
            set 
            { 
                _gridViewType = value;
                CreateGrid();
            }
        }
        
        

        public int RowCount { get { return GridSize.RowCount; } }
        public int ColCount { get { return GridSize.ColCount; } }

        public void Reset()
        {
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColCount; c++)
                {
                    var cell = cells[r, c];
                    SetCell(cell, false);
                }
            }
        }

        public void ChangeCells(CellInfo[] cellsChanged)
        {
            foreach (var cell in cellsChanged)
            {
                SetCell(cell.Row, cell.Col, cell.Live);
            }
        }
        
        public Size CellSize { get; private set; }
        Ellipse[,] cells;

        private Ellipse CreateCell(int r, int c)
        {
            if (GridViewType == LifeUIWPF.GridViewType.GridLines)
                CreateGrid(r, c);
            Ellipse el = new Ellipse();
            el.Width = CellSize.Width - CELLS_SPACING;
            el.Height = CellSize.Height - CELLS_SPACING;
            el.Stroke = (GridViewType == GridViewType.DeadGrayCells) ? new SolidColorBrush(Colors.Black) : null;
            el.StrokeThickness = 1;
            Canvas.SetLeft(el, c * (CellSize.Width) + CELLS_SPACING/2);
            Canvas.SetTop(el, r * (CellSize.Height) + CELLS_SPACING/2);
            el.MouseDown += el_MouseDown;
            el.Tag = false;
            canvas.Children.Add(el);
            return el;
        }

        private void CreateGrid(int r, int c)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.LightGray);
            rect.StrokeThickness = 1;
            rect.Width = CellSize.Width;
            rect.Height = CellSize.Height;
            Canvas.SetLeft(rect, c * rect.Width);
            Canvas.SetTop(rect, r * rect.Height);
            canvas.Children.Add(rect);
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
            if (live)
                cell.Fill = liveBrush;
            else
                cell.Fill = deadBrush;
        }
        
        private void ChangeCell(Ellipse cell)
        {
            bool live = !(bool)cell.Tag;
            SetCell(cell, live);            
        }

        private void CreateGrid()
        {
            canvas.Children.Clear();
            CellSize = new Size(canvas.ActualWidth / ColCount, canvas.ActualHeight / RowCount);

            deadBrush = (GridViewType == GridViewType.GridLines) ? Background : deadGrayCellsBrush;

            cells = new Ellipse[RowCount, ColCount];
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateGrid();
        }
    }

    public enum GridViewType
    {
        GridLines,
        DeadGrayCells
    }
}
