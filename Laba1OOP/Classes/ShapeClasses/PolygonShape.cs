using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections.Generic;

public class PolygonShape : MyShape
{
    public List<Point> Points { get; set; } = new List<Point>(); 

    private Point? tempPoint;

    public PolygonShape(float x1, float y1, Brush backgroundColor, Brush strokeColor, double strokeThickness)
        : base(x1, y1, backgroundColor, strokeColor, strokeThickness)
    {
        Points.Add(new Point(x1, y1)); 
    }

    public override bool IsMultiPoint => true;

    public override void AddPoint(float x, float y)
    {
        Points.Add(new Point(x, y)); 
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
            StrokeThickness = StrokeThickness,
            IsHitTestVisible = false
        };

        foreach (var point in Points) 
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
