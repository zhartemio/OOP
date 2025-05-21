using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public abstract class MyShape
{
    protected float X1;
    protected float Y1;
    protected Brush BackgroundColor;
    protected Brush StrokeColor;
    protected double StrokeThickness;

    protected MyShape(
        float x1, float y1,
        Brush backgroundColor, Brush strokeColor, double strokeThickness)
    {
        X1 = x1;
        Y1 = y1;
        BackgroundColor = backgroundColor;
        StrokeColor = strokeColor;
        StrokeThickness = strokeThickness;
    }

    public abstract void Draw(Canvas canvas);
    public abstract void Update(float xRight, float yRight);

    // Новое:
    public virtual bool IsMultiPoint => false;
    public virtual void AddPoint(float x, float y) { }
}
