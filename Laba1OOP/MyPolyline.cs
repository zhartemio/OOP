using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

public class MyPolyline : MyShape
{
    private System.Collections.Generic.List<Point> _points = new System.Collections.Generic.List<Point>();

    public MyPolyline(float xLeft, float yLeft, float xRight, float yRight, Brush backgroundColor, Brush strokeColor)
        : base(xLeft, yLeft, xRight, yRight, backgroundColor, strokeColor)
    {
        // Первые две точки
        _points.Add(new Point(xLeft, yLeft));
        _points.Add(new Point(xRight, yRight));
    }

    public override void Update(float xRight, float yRight)
    {
        // Добавляем новую точку и обновляем конечные координаты
        _points.Add(new Point(xRight, yRight));
        XRight = xRight;
        YRight = yRight;
    }

    public override void Draw(Canvas canvas)
    {
        var polyline = new Polyline
        {
            Stroke = Brushes.Black, // Настройте цвет
            StrokeThickness = 2     // Настройте толщину
        };

        // Конвертируем точки в коллекцию для WPF
        var pointCollection = new PointCollection();
        foreach (var p in _points)
        {
            pointCollection.Add(p);
        }
        polyline.Points = pointCollection;

        canvas.Children.Add(polyline);
    }
}