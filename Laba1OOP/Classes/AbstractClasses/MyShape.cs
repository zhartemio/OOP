using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json; // Не забудьте добавить ссылку на Newtonsoft.Json

public abstract class MyShape
{
    public float X1 { get; set; }
    public float Y1 { get; set; }
    public Brush BackgroundColor { get; set; }
    public Brush StrokeColor { get; set; }
    public double StrokeThickness { get; set; }

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

    public virtual bool IsMultiPoint => false;
    public virtual void AddPoint(float x, float y) { }

    
    public virtual string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}