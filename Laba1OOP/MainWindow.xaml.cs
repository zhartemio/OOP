using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Media;

namespace Laba1OOP
{
    public partial class MainWindow : Window
    {
        

        private bool isDrawing = false;
        private float startX, startY;
        private List<MyShape> shapes;
        private List<MyShape> removedShapes;
        private MyShape currentShape;
        private string selectedShape = "";

        public MainWindow()
        {
            InitializeComponent();
            shapes = new List<MyShape>();
            removedShapes = new List<MyShape>();
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(MyCanvas);
            float mouseX = (float)position.X;
            float mouseY = (float)position.Y;

            if (!isDrawing)
            {
                
                if (string.IsNullOrEmpty(selectedShape)) return;
                isDrawing = true;
                removedShapes.Clear();
                RedoButton.IsEnabled = false;
                startX = mouseX;
                startY = mouseY;
                Brush backgroundColor = BackColor.Background;
                Brush strokeColor = BackColor.BorderBrush;

                if (selectedShape == "Rectangle")
                {
                    currentShape = new RectangleShape(startX, startY, startX, startY, backgroundColor, strokeColor);
                }
                else if (selectedShape == "Ellipse")
                {
                    currentShape = new EllipseShape(startX, startY, startX, startY,backgroundColor, strokeColor);
                }
                else if (selectedShape == "Line")
                {
                    currentShape = new LineShape(startX, startY, startX, startY, backgroundColor, strokeColor);
                }
            }
            else
            {
                isDrawing = false;
                if (currentShape != null)
                {
                    currentShape.Update(mouseX, mouseY);
                    shapes.Add(currentShape);
                   
                }
                RedrawCanvas();
                UndoButton.IsEnabled = true;

            }
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;

            var position = e.GetPosition(MyCanvas);
            float endX = (float)position.X;
            float endY = (float)position.Y;

            endX = Math.Max(0, Math.Min(endX, (float)MyCanvas.ActualWidth));
            endY = Math.Max(0, Math.Min(endY, (float)MyCanvas.ActualHeight));

            if (currentShape != null)
            {
                currentShape.Update(endX, endY);

                RedrawCanvas(); 
                currentShape.Draw(MyCanvas);
            }
        }



        private void RedrawCanvas()
        {
            MyCanvas.Children.Clear();
            foreach (var shape in shapes)
            {
                shape.Draw(MyCanvas);
            }
        }

        private void HandleChecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton currentButton && currentButton.IsChecked == true)
            {
                StackPanel shapeButtons = ShapeButtons;

                foreach (var child in shapeButtons.Children)
                {
                    if (child is ToggleButton toggleButton && toggleButton != currentButton)
                    {
                        toggleButton.IsChecked = false;
                    }
                }

                selectedShape = currentButton.Content?.ToString() ?? string.Empty;
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            shapes.Add(removedShapes.Last());
            removedShapes.Remove(removedShapes.Last());
            if (removedShapes.Count == 0)
            {
                RedoButton.IsEnabled = false;
            }
            RedrawCanvas();
            UndoButton.IsEnabled = true;
            DeactivateToogleButtons();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            removedShapes.Add(shapes.Last());
            shapes.Remove(shapes.Last());
            if (shapes.Count == 0)
            {
                UndoButton.IsEnabled = false;
            }
            RedrawCanvas();
            RedoButton.IsEnabled = true;
            DeactivateToogleButtons();
            
        }

        private void DeactivateToogleButtons()
        {
            foreach (var child in ShapeButtons.Children)
            {
                if (child is ToggleButton toggleButton)
                {
                    toggleButton.IsChecked = false;
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (shapes.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы желаете сохранить фигуры перед закрытием окна?",
                    "Сохранение",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; 
                }

            }
        }

        private void SaveShapesToFile()
        {

        }


        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveShapesToFile();
        }


        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            selectedShape = "";
        }


        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                BackColor.Background = button.Background;
            }
        }

        private void StrokeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                BackColor.BorderBrush = button.Background;
            }
        }
    }

}

