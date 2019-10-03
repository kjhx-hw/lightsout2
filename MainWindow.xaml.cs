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

namespace lightsout2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window {
        private LightsOutGame game;
        public MainWindow() {
            InitializeComponent();

            game = new LightsOutGame();
            CreateGrid();
            DrawGrid();
        }

        private void CreateGrid() {
            int rectSize = (int)boardCanvas.Width / game.GridSize;

            for (int r = 0; r < game.GridSize; r++) {
                for (int c = 0; c < game.GridSize; c++) {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.White;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = Brushes.Black;

                    rect.Tag = new Point(r, c);

                    rect.MouseLeftButtonDown += Rect_MouseLeftButtonDown;

                    Canvas.SetTop(rect, r * rectSize);
                    Canvas.SetLeft(rect, c * rectSize);

                    boardCanvas.Children.Add(rect);
                }
            }
        }

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Rectangle rect = sender as Rectangle;
            var rowCol = (Point)rect.Tag;
            int row = (int)rowCol.X;
            int col = (int)rowCol.Y;

            game.Move(row, col);

            DrawGrid();

            if (game.IsGameOver()) {
                MessageBox.Show("You've won!");
                // game.NewGame();
                // DrawGrid();
            }

            e.Handled = true;
        }

        private void DrawGrid() {
            int index = 0;

            for (int r = 0; r < game.GridSize; r++) {
                for (int c = 0; c < game.GridSize; c++) {
                    Rectangle rect = boardCanvas.Children[index] as Rectangle;
                    index++;

                    if (game.GetGridValue(r,c)) {
                        rect.Fill = Brushes.White;
                        rect.Stroke = Brushes.Black;
                    } else {
                        rect.Fill = Brushes.Black;
                        rect.Stroke = Brushes.White;
                    }
                }
            }
        }

        private void AboutCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            game.NewGame();
            DrawGrid();
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            try {
                e.CanExecute = game.IsGameOver();
            } catch (Exception) {
                e.CanExecute = false;
            }
        }
    }
}
