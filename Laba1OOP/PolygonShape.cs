using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class PolygonShape : MyShape
{
    private List<Point> points;
    private Point? tempPoint;

    public PolygonShape(float startX, float startY, Brush backgroundColor, Brush strokeColor)
        : base(startX, startY, startX, startY, backgroundColor, strokeColor)
    {
        points = new List<Point>();
        points.Add(new Point(startX, startY));
    }

    public void AddPoint(float x, float y)
    {
        points.Add(new Point(x, y));
        tempPoint = null;
    }

    public override void Update(float xRight, float yRight)
    {
        tempPoint = new Point(xRight, yRight);
    }

    public override void Draw(Canvas canvas)
    {
        var polygon = new Polygon
        {
            Stroke = StrokeColor,
            Fill = BackgroundColor,
            StrokeThickness = 2,
            IsHitTestVisible = false
        };

        foreach (var point in points)
        {
            polygon.Points.Add(point);
        }

        if (tempPoint.HasValue)
        {
            polygon.Points.Add(tempPoint.Value);
        }

        canvas.Children.Add(polygon);
    }
}
