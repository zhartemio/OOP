using System.Windows.Media;
using Laba1OOP.Classes.AbstractClasses;

namespace Trapezoid
{
    public class TrapezoidPlugin : IShapePlugin
{
    public string Name => "Trapezoid";

    public MyShape CreateShape(float x1, float y1, Brush fill, Brush stroke, double thickness)
    {
        return new TrapezoidShape(x1, y1, fill, stroke, thickness);
    }
}
}
