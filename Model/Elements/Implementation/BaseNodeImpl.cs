using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Model.Elements.Interface;

namespace Model.Elements.Implementation
{
    [XmlType("Node")]
    public class BaseNodeImpl : BaseElementImpl, IBaseNode
    {
        public BaseNodeImpl() { }
        public string Color { get; set; }
        public double Opacity { get; set; } = 1;
        public List<BaseConnectionImpl> Connections { get; } = new List<BaseConnectionImpl>();

        public BaseNodeImpl(double left, double top)
        {
            Width = 20;
            Height = 20;
            Left = 0;
            Top = 0;
            Color = "Brown";
            
        }

        public void AddConnection(BaseConnectionImpl connection)
        {
            if (Connections.Count >= 3) throw new Exception("A node cannot have more than 3 connecions!");

            Connections.Add(connection);
        }
    }
}
