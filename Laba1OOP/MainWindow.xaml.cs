using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Laba1OOP
{
    public partial class MainWindow : Window
    {
        private bool isDrawing = false;
        private float startX, startY;

        private List<MyShape> shapes = new();
        private List<MyShape> removedShapes = new();

        private UndoRedoManager<MyShape> undoRedoManager;

        private MyShape? currentShape;
        private string selectedShape = "";
        private double thickness;

        private Dictionary<string, Func<float, float, Brush, Brush, double, MyShape>> shapeCreators;

        public MainWindow()
        {
            InitializeComponent();

            StrokeThicknessComboBox.SelectedIndex = 0;

            undoRedoManager = new UndoRedoManager<MyShape>(shapes, removedShapes);

            shapeCreators = new Dictionary<string, Func<float, float, Brush, Brush, double, MyShape>>
            {
                ["Rectangle"] = (x, y, fill, stroke, th) => new RectangleShape(x, y, x, y, fill, stroke, th),
                ["Ellipse"]   = (x, y, fill, stroke, th) => new EllipseShape(x, y, x, y, fill, stroke, th),
                ["Line"]      = (x, y, fill, stroke, th) => new LineShape(x, y, x, y, fill, stroke, th),
                ["Polyline"]  = (x, y, fill, stroke, th) => new PolylineShape(x, y, fill, stroke, th),
                ["Polygon"]   = (x, y, fill, stroke, th) => new PolygonShape(x, y, fill, stroke, th)
            };

            UpdateUndoRedoButtons();
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MyCanvas);
            float mouseX = (float)pos.X;
            float mouseY = (float)pos.Y;

            if (e.ChangedButton == MouseButton.Right && isDrawing && currentShape != null)
            {
                isDrawing = false;

                if (currentShape.IsMultiPoint)
                {
                    currentShape.AddPoint(mouseX, mouseY);
                }
                else
                {
                    currentShape.Update(mouseX, mouseY);
                }

                undoRedoManager.Add(currentShape);
                currentShape = null;
                RedrawCanvas();
                UpdateUndoRedoButtons();
                return;
            }

            if (!isDrawing)
            {
                if (string.IsNullOrEmpty(selectedShape)) return;

                isDrawing = true;

                // Начинаем новое действие — очищаем redo через UndoRedoManager
                removedShapes.Clear(); // либо добавить метод в UndoRedoManager, если хочешь
                RedoButton.IsEnabled = false;

                startX = mouseX;
                startY = mouseY;

                Brush bg = BackColor.Background;
                Brush stroke = BackColor.BorderBrush;

                if (shapeCreators.TryGetValue(selectedShape, out var factory))
                {
                    currentShape = factory(mouseX, mouseY, bg, stroke, thickness);
                }
            }
            else if (currentShape != null)
            {
                if (currentShape.IsMultiPoint)
                {
                    currentShape.AddPoint(mouseX, mouseY);
                    RedrawCanvas();
                    currentShape.Draw(MyCanvas);
                }
                else
                {
                    isDrawing = false;
                    currentShape.Update(mouseX, mouseY);
                    undoRedoManager.Add(currentShape);
                    currentShape = null;
                    RedrawCanvas();
                    UpdateUndoRedoButtons();
                }
            }
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || currentShape == null) return;

            var pos = e.GetPosition(MyCanvas);
            float endX = Math.Clamp((float)pos.X, 0, (float)MyCanvas.ActualWidth);
            float endY = Math.Clamp((float)pos.Y, 0, (float)MyCanvas.ActualHeight);

            RedrawCanvas();

            currentShape.Update(endX, endY);
            currentShape.Draw(MyCanvas);
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
                foreach (var child in ShapeButtons.Children)
                {
                    if (child is ToggleButton toggleButton && toggleButton != currentButton)
                    {
                        toggleButton.IsChecked = false;
                    }
                }

                selectedShape = currentButton.Content?.ToString() ?? string.Empty;
            }
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            selectedShape = "";
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
                BackColor.Background = button.Background;
        }

        private void StrokeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
                BackColor.BorderBrush = button.Background;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box &&
                box.SelectedItem is ComboBoxItem item &&
                double.TryParse(item.Tag?.ToString(), out double strokeThickness))
            {
                thickness = strokeThickness;
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            undoRedoManager.Redo();
            RedrawCanvas();
            UpdateUndoRedoButtons();
            DeactivateToggleButtons();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            undoRedoManager.Undo();
            RedrawCanvas();
            UpdateUndoRedoButtons();
            DeactivateToggleButtons();
        }

        private void UpdateUndoRedoButtons()
        {
            UndoButton.IsEnabled = undoRedoManager.CanUndo;
            RedoButton.IsEnabled = undoRedoManager.CanRedo;
        }

        private void DeactivateToggleButtons()
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
            if (shapes.Count == 0) return;

            var result = MessageBox.Show(
                "Вы желаете сохранить фигуры перед закрытием окна?",
                "Сохранение",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (result == MessageBoxResult.Yes)
            {
                SaveShapesToFile();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) => SaveShapesToFile();

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Close();

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // Реализация загрузки фигур по желанию
        }

        private void SaveShapesToFile()
        {
            // Реализация сохранения фигур по желанию
        }
    }
}
