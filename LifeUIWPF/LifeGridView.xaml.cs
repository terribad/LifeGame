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

namespace LifeUIWPF
{
    /// <summary>
    /// Interaction logic for LifeGridView.xaml
    /// </summary>
    public partial class LifeGridView : UserControl
    {
        public LifeGridView():this(10,10)
        {
        }

        public LifeGridView(int rowCount, int colCount)
        {
            InitializeComponent();
            this.RowCount = rowCount;
            this.ColCount = colCount;
        }

        public int RowCount { get; private set; }
        public int ColCount { get; private set; }

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

        readonly SolidColorBrush liveBrush = new SolidColorBrush(Colors.LightCoral);
        Brush deadBrush = new SolidColorBrush(Colors.LightGray);        
        readonly int cellSize = 30;
        Ellipse[,] cells;

        private Ellipse CreateCell(int r, int c)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.LightGray);
            rect.StrokeThickness = 1;

            rect.Width = cellSize + 6;
            rect.Height = cellSize + 6;
            Canvas.SetLeft(rect, c * rect.Width);
            Canvas.SetTop(rect, r * rect.Height);
            
            Ellipse el = new Ellipse();
            el.Width = cellSize;
            el.Height = cellSize;

            Canvas.SetLeft(el, c * (rect.Width)+3);
            Canvas.SetTop(el, r * (rect.Height)+3);
            el.MouseDown += el_MouseDown;
            el.Tag = false;
            canvas.Children.Add(rect);
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

        private void CreateGridView()
        {
            Background = Brushes.White;
            Grid.SetRow(this, 1);
            Grid.SetColumn(this, 0);
            deadBrush = Background;
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
            CreateGridView();
        }
    }
}
