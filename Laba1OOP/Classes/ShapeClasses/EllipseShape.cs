using Laba1OOP;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class EllipseShape : TwoPointShape
{
    public EllipseShape(float xLeft, float yLeft, float xRight, float yRight, Brush backgroundColor, Brush strokeColor, double strokeThickness)
        : base( xLeft,  yLeft,  xRight,  yRight, backgroundColor, strokeColor, strokeThickness)
    {
    }

    public override void Draw(Canvas canvas)
    {
        float width = Math.Abs(X1 - X2);
        float height = Math.Abs(Y1 - Y2);

        var ellipse = new Ellipse
        {
            Width = width,
            Height = height,
            StrokeThickness = StrokeThickness,
            Fill = BackgroundColor,
            Stroke = StrokeColor,
            IsHitTestVisible = false
        };

        Canvas.SetLeft(ellipse, Math.Min(X1,X2));
        Canvas.SetTop(ellipse, Math.Min(Y1,Y2));

        canvas.Children.Add(ellipse);
    }

    public override void Update(float x2, float y2)
    {
        X2 = x2;
        Y2 = y2;
    }
}
