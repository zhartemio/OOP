using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class LineShape : MyShape
{
    public LineShape(float xStart, float yStart, float xEnd, float yEnd, Brush backgroundColor, Brush strokeColor)
        : base(xStart, yStart, xEnd, yEnd, backgroundColor, strokeColor)
    {
    }

    public override void Draw(Canvas canvas)
    {
        var line = new Line
        {
            X1 = XLeft,
            Y1 = YLeft,
            X2 = XRight,
            Y2 = YRight,
            Fill = BackgroundColor,
            StrokeThickness = 2,
            Stroke = StrokeColor,
            IsHitTestVisible = false
        };

        canvas.Children.Add(line);
    }

    public override void Update(float xRight, float yRight)
    {
        XRight = xRight;
        YRight = yRight;
    }
}
