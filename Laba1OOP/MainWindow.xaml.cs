
using Laba1OOP.Classes.AbstractClasses;
using Laba1OOP.Classes.Managers;
using Microsoft.Win32;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
        private int pluginCount = 0;

        private List<MyShape> shapes = new();
        private List<MyShape> removedShapes = new();

        private IShapePlugin currentPlugin;

        private UndoRedoManager<MyShape> undoRedoManager;
        private FileManager<MyShape> fileManager;


        private MyShape? currentShape;
        private string selectedShape = "";
        private double thickness;

        private Dictionary<string, Func<float, float, Brush, Brush, double, MyShape>> shapeCreators;

        public MainWindow()
        {
            InitializeComponent();

            StrokeThicknessComboBox.SelectedIndex = 0;

            undoRedoManager = new UndoRedoManager<MyShape>(shapes, removedShapes);
            fileManager = new FileManager<MyShape>(shapes);

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

                removedShapes.Clear();
                RedoButton.IsEnabled = false;

                startX = mouseX;
                startY = mouseY;

                Brush bg = BackColor.Background;
                Brush stroke = BackColor.BorderBrush;
                string selected = selectedShape;

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
                foreach (var child in PluginPanel.Children)
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            fileManager.SaveElementsToFile();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Close();

        private void SaveShapesToFile()
        {
            fileManager.SaveElementsToFile();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            shapes.Clear();
            removedShapes.Clear();
            fileManager.LoadElementsFromFile();
            RedrawCanvas();
            UpdateUndoRedoButtons();
        }



        public void LoadPlugin_Click(object s, RoutedEventArgs e)
        {

            if (pluginCount == 3) return;
            var dialog = new OpenFileDialog
            {
                Filter = "DLL файлы (*.dll)|*.dll"
            };

            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                Assembly assembly = Assembly.LoadFrom(path);

                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IShapePlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IShapePlugin plugin = (IShapePlugin)Activator.CreateInstance(type);
                        shapeCreators[plugin.Name] = (x, y, fill, stroke, thickness) =>
                            plugin.CreateShape(x, y, fill, stroke, thickness);
                        AddPluginButton(plugin);
                    }
                }
            }
            pluginCount++;
        }
        private void AddPluginButton(IShapePlugin plugin)
        {
            var button = new ToggleButton
            {
                Content = plugin.Name,
                Width = 40,
                Height = 40,
                Margin = new Thickness(5)
            };

            button.Checked += (s, e) =>
            {
                currentPlugin = plugin;
                HandleChecked(s, e);
                
                button.IsChecked = true;
            };

            button.Unchecked += (s, e) =>
            {
                if (currentPlugin == plugin)
                    currentPlugin = null;
            };

            PluginPanel.Children.Add(button);
        }


    }
}
