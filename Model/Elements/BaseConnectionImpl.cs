using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class BaseConnectionImpl : BaseElementImpl, IBaseConnection
    {
        public List<IBaseNode> Nodes { get; }
        public string Color { get; set; }
        public double Left2 { get; set; }
        public double Top2 { get; set; }
        public BaseConnectionImpl(IBaseNode node1, IBaseNode node2)
        {
            this.Left = node1.Left;
            this.Top = node1.Top;
            this.Left2 = node2.Left;
            this.Top2 = node2.Top;
            Nodes.Add(node1);
            Nodes.Add(node2);
            this.Color = "Yellow";
        }
    }
    
}
