
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Model.Elements 
{
    [XmlInclude(typeof(StationImpl))]
    [XmlInclude(typeof(ConnectionPointImpl))]
    [XmlInclude(typeof(BaseNodeImpl))]
    [XmlInclude(typeof(BaseConnectionImpl))]
    public abstract class BaseElementImpl : IBaseElement
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

        public BaseElementImpl() { }
    }
}
