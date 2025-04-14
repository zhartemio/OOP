using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class RectangleShape : MyShape
{
    public RectangleShape(float xLeft, float yLeft, float xRight, float yRight, Brush backgroundColor, Brush strokeColor)
        : base(xLeft, yLeft, xRight, yRight, backgroundColor, strokeColor) 
    {
    }

    public override void Draw(Canvas canvas)
    {
        float width = Math.Abs(XRight - XLeft);
        float height = Math.Abs(YRight - YLeft);

        var rectangle = new Rectangle
        {
            Width = width,
            Height = height,
            StrokeThickness = 2,
            Fill = BackgroundColor,
            Stroke = StrokeColor,
            IsHitTestVisible = false
        };

        Canvas.SetLeft(rectangle, Math.Min(XLeft, XRight));
        Canvas.SetTop(rectangle, Math.Min(YLeft, YRight));

        canvas.Children.Add(rectangle);
    }

    public override void Update(float xRight, float yRight)
    {
        XRight = xRight;
        YRight = yRight;
    }
}
