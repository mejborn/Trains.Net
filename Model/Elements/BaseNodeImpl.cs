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
            Color = "Cyan";
        }

        public bool AddConnection(IBaseConnection connection)
        {
            if (!Connections.Contains(connection))
            {
                if (Connections.Count < 2)
                {
                    Connections.Add(connection);
                    return true;
                }
                else return false; // ÉRROR: Not more than 2 allowed
            }
            else return false; // ERROR: Already exists

        }

       
    }
}
