using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class BaseNodeImpl : BaseElementImpl, IBaseNode
    {
        public string Color { get; set; }
        public List<IBaseConnection> Connections { get; }

        public BaseNodeImpl(double left, double top)
        {
            Connections = new List<IBaseConnection>();
            this.Left = left;
            this.Top = Top;
            Color = "White";
        }

        public void AddConnection(IBaseConnection connection)
        {
            if (Connections.Count >= 2) throw new Exception("A node cannot have more than 2 connecions!");

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
