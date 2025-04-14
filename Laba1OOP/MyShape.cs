using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

public abstract class MyShape
{
    protected float XLeft;
    protected float XRight;
    protected float YLeft;
    protected float YRight;
    protected Brush BackgroundColor;
    protected Brush StrokeColor;

    public MyShape(float xLeft, float yLeft, float xRight, float yRight, Brush backgroundColor, Brush strokeColor)
    {
        XLeft = xLeft;
        XRight = xRight;
        YLeft = yLeft;
        YRight = yRight;
        BackgroundColor = backgroundColor;
        StrokeColor = strokeColor;
    }
    public abstract void Draw(Canvas canvas);
    public abstract void Update(float xRight, float yRight);
}