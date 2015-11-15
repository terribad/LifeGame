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

        static readonly Brush liveBrush = new SolidColorBrush(Colors.LightCoral);
        static readonly Brush deadGrayCellsBrush = new SolidColorBrush(Colors.LightGray);
        static readonly Brush lineBrush = new SolidColorBrush(Colors.LightGray);
        Brush deadBrush = deadGrayCellsBrush;
        Ellipse[,] cells;
        Line[] lines;
        public Size CellSize { get; private set; }
        

        private GridSize _gridSize = new GridSize(10,10);
        public GridSize GridSize
        {
            get { return _gridSize; }
            set
            {
                _gridSize = value;
                UpdateUI();
            }
        }

        private GridViewType _gridViewType = GridViewType.GridLines;
        public GridViewType GridViewType
        {
            get { return _gridViewType; }
            set 
            { 
                _gridViewType = value;
                deadBrush = (GridViewType == GridViewType.GridLines) ? Background : deadGrayCellsBrush;
                UpdateUI();
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


        void UpdateUI()
        {
            foreach (var line in canvas.Children.OfType<Line>())
            {
                if (GridViewType == GridViewType.DeadGrayCells)
                    line.Stroke = null;
                else
                    line.Stroke = lineBrush;
            }

            foreach (var cell in canvas.Children.OfType<Ellipse>())
            {
                if (GridViewType == GridViewType.DeadGrayCells)
                {
                    cell.Stroke = new SolidColorBrush(Colors.Black);
                    cell.Fill = deadBrush;
                }
                else
                {
                    cell.Stroke = null;
                    cell.Fill = canvas.Background;
                    //cell.Fill = Brushes.White;
                }
            }
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
            canvas.Children.Add(li);
        }

        private void CreateHorizontalLine(int r)
        {
            Line li = new Line();
            li.X1 = 0;
            li.X2 = canvas.ActualWidth;
            li.Y1 = r * CellSize.Height;
            li.Y2 = li.Y1;
            canvas.Children.Add(li);
        }

        private void CreateGrid()
        {
            canvas.Children.Clear();
            CellSize = new Size(canvas.ActualWidth / ColCount, canvas.ActualHeight / RowCount);

            //deadBrush = (GridViewType == GridViewType.GridLines) ? Background : deadGrayCellsBrush;

            cells = new Ellipse[RowCount, ColCount];
            lines = new Line[(RowCount - 2) * (ColCount - 2)];
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
            UpdateUI();
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
