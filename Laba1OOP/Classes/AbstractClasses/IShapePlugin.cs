using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Laba1OOP.Classes.AbstractClasses
{
    public interface IShapePlugin
    {
        string Name { get; } // Название фигуры
        MyShape CreateShape(float x1, float y1, Brush fill, Brush stroke, double thickness);
    }
}
