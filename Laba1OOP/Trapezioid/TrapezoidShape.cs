using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Trapezoid
{
  
    public class TrapezoidShape : MyShape
    {
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public TrapezoidShape(float x1, float y1, Brush backgroundColor, Brush strokeColor, double strokeThickness)
            : base(x1, y1, backgroundColor, strokeColor, strokeThickness)
        {
        }

        public override void Update(float xRight, float yRight)
        {
            X2 = xRight;
            Y2 = yRight;
        }

        public override void Draw(Canvas canvas)
        {
            
            Point topLeft = new Point(X1 + (X2 - X1) * 0.25, Y1);
            Point topRight = new Point(X2 - (X2 - X1) * 0.25, Y1);
            Point bottomRight = new Point(X2, Y2);
            Point bottomLeft = new Point(X1, Y2);

            Polygon trapezoid = new Polygon
            {
                Fill = BackgroundColor,
                Stroke = StrokeColor,
                StrokeThickness = StrokeThickness,
                Points = new PointCollection { topLeft, topRight, bottomRight, bottomLeft },
                IsHitTestVisible = false
            };

            canvas.Children.Add(trapezoid);
        }
    }


}
