

using System;

namespace Model.Elements
{
    public abstract class BaseElementImpl : IBaseElement
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;
        
    }
}
