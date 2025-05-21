using Laba1OOP;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class RectangleShape : TwoPointShape
{
    public RectangleShape(float x1, float y1, float x2, float y2, Brush backgroundColor, Brush strokeColor, double strokeThickness)
        : base(x1, y1, x2, y2, backgroundColor, strokeColor, strokeThickness) 
    {
    }

    public override void Draw(Canvas canvas)
    {
        float width = Math.Abs(X1 - X2);
        float height = Math.Abs(Y1 - Y2);

        var rectangle = new Rectangle
        {
            Width = width,
            Height = height,
            StrokeThickness = StrokeThickness,
            Fill = BackgroundColor,
            Stroke = StrokeColor,
            IsHitTestVisible = false
        };

        Canvas.SetLeft(rectangle, Math.Min(X1, X2));
        Canvas.SetTop(rectangle, Math.Min(Y1, Y2));

        canvas.Children.Add(rectangle);
    }

    public override void Update(float x2, float y2)
    {
        X2 = x2;
        Y2 = y2;
    }
}
