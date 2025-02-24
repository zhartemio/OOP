using System;
using System.Drawing;

public class Circle : Shape
{
    private double x_left;
    private double y_left;
    private double radius;

    public Circle(double x_left, double y_left, double radius)
	{
        this.x_left = x_left;
        this.y_left = y_left;
        this.radius = radius;
    }



    public override void Draw(Canvas canvas)
    {
        using (Brush brush = new SolidBrush(Color.Black))
        {
            RectangleF rectangle = new RectangleF((float)(x_left - radius), (float)(y_left - radius), (float)(radius * 2), (float)(radius * 2));
            canvas.FillEllipse(brush, rectangle);
        }
    }
}
