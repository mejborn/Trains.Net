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
            Left = 0;
            Top = 0;
            Color = "Red";
            
        }

        public void AddConnection(BaseConnectionImpl connection)
        {
            if (Connections.Count >= 3) throw new Exception("A node cannot have more than 3 connecions!");

            Connections.Add(connection);

            /*
            
            bool connectionsEquals = false;

            foreach (var curConnect in Connections)
            {
                if (curConnect.node1 == connection.node1 && curConnect.node2 == connection.node2)
                {
                    connectionsEquals = true;
                }
            }

            if (!connectionsEquals)
            {
                if (Connections.Count < 2)
                {
                    Connections.Add(connection);
                    return 0;
                }
                else return 2; // ÉRROR: Not more than 2 allowed (ERROR-CODE: 2)
            }
            else return 1; // ERROR: Already exists (ERROR-CODE: 1)
            */
        }

       
    }
}
