using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;


public class PolylineShape : MyShape
{
    private List<Point> points;
    private Point? tempPoint;

    public PolylineShape(float x1, float y1, Brush backgroundColor, Brush strokeColor, double strokeThickness)
        : base(x1, y1, backgroundColor, strokeColor, strokeThickness)
    {
        points = new List<Point>();
        points.Add(new Point(x1, y1));
    }

    public override bool IsMultiPoint => true;

    public override void AddPoint(float x, float y)
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
        var polyline = new Polyline
        {
            Stroke = StrokeColor,
            StrokeThickness = StrokeThickness,
            Fill = Brushes.Transparent,
            IsHitTestVisible = false
        };

        foreach (var point in points)
        {
            polyline.Points.Add(point);
        }

        if (tempPoint.HasValue)
        {
            polyline.Points.Add(tempPoint.Value);
        }

        canvas.Children.Add(polyline);
    }
}
