using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class LineShape : TwoPointShape
{
    public LineShape(float x1, float y1, float x2,  float y2, Brush backgroundColor, Brush strokeColor, double strokeThickness)
        : base(x1, y1, x2, y2, backgroundColor, strokeColor, strokeThickness)
    {
    }

    public override void Draw(Canvas canvas)
    {
        var line = new Line
        {
            X1 = X1,
            Y1 = Y1,
            X2 = X2,
            Y2 = Y2,
            Fill = BackgroundColor,
            StrokeThickness = StrokeThickness,
            Stroke = StrokeColor,
            IsHitTestVisible = false
        };

        canvas.Children.Add(line);
    }

    public override void Update(float x2, float y2)
    {
        X2 = x2;
        Y2 = y2;
    }
}
