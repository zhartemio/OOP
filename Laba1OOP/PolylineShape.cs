using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class PolylineShape : MyShape
{
    private List<Point> points;

    public PolylineShape(float startX, float startY, Brush backgroundColor, Brush strokeColor)
        : base(startX, startY, startX, startY, backgroundColor, strokeColor)
    {
        points = new List<Point>();
        points.Add(new Point(startX, startY));
    }

    public override void Draw(Canvas canvas)
    {
        var polyline = new Polyline
        {
            Stroke = StrokeColor,
            StrokeThickness = 2,
            IsHitTestVisible = false
        };

        foreach (var point in points)
        {
            polyline.Points.Add(point);
        }

        canvas.Children.Add(polyline);
    }

    public override void Update(float xRight, float yRight)
    {         
        XRight = xRight;
        YRight = yRight;
    }
}
