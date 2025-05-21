using System.Windows.Media;

public abstract class TwoPointShape : MyShape
{
    protected float X2;
    protected float Y2;

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
}
