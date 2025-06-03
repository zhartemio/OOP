using System.Windows.Controls;
using System.Windows.Media;

public abstract class TwoPointShape : MyShape
{
    public float X2 { get; set; }
    public float Y2 { get; set; }

    protected TwoPointShape(
        float x1, float y1, float x2, float y2,
        Brush backgroundColor, Brush strokeColor, double strokeThickness)
        : base(x1, y1, backgroundColor, strokeColor, strokeThickness)
    {
        X2 = x2;
        Y2 = y2;
    }

    public override void Update(float xRight, float yRight)
    {
        X2 = xRight;
        Y2 = yRight;
    }

    public override void Draw(Canvas canvas)
    {
        var rectangle = new System.Windows.Shapes.Rectangle
        {
            Width = Math.Abs(X2 - X1),
            Height = Math.Abs(Y2 - Y1),
            Fill = BackgroundColor,
            Stroke = StrokeColor,
            StrokeThickness = StrokeThickness
        };

        Canvas.SetLeft(rectangle, Math.Min(X1, X2));
        Canvas.SetTop(rectangle, Math.Min(Y1, Y2));

        canvas.Children.Add(rectangle);
    }
}