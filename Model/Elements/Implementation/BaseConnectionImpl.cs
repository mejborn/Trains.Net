using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Interface;

namespace Model.Elements.Implementation
{
    [Serializable]
    public class BaseConnectionImpl : BaseElementImpl, IBaseConnection
    {
        public BaseConnectionImpl() { }
        public IBaseNode node1 { get; }
        public IBaseNode node2 { get; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public string Color { get; set; }
        public ConnectionPointImpl CP1 { get; set; }
        public ConnectionPointImpl CP2 { get; set; }
        public BaseConnectionImpl(IBaseNode node1, IBaseNode node2, ConnectionPointImpl cp1, ConnectionPointImpl cp2)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.Left = 0;
            this.Top = 0;
            this.CP1 = cp1;
            this.CP2 = cp2;
            CalculatePosition();

            Color = "Brown";

           
        }

        public void CalculatePosition()
        {
            if (CP1 != null && CP2 != null)
            {
                X1 = node1.Left + CP1.Left + (CP1.Width / 2);
                X2 = node2.Left + CP2.Left + (CP2.Width / 2);
                Y1 = node1.Top + CP1.Top + (CP1.Height / 2);
                Y2 = node2.Top + CP2.Top + (CP2.Height / 2);
            } else if (CP1 == null & CP2 != null)
            {
                X1 = node1.Left + (node1.Width / 2);
                X2 = node2.Left + CP2.Left + (CP2.Width / 2);
                Y1 = node1.Top + (node1.Height / 2);
                Y2 = node2.Top + CP2.Top + (CP2.Height / 2);
            }
            else if (CP1 != null && CP2 == null)
            {
                X1 = node1.Left + CP1.Left + (CP1.Width/2);
                X2 = node2.Left + (node2.Width/2);
                Y1 = node1.Top + CP1.Top + (CP1.Height/2);
                Y2 = node2.Top + (node2.Height/2);
            }
            else
            {
                X1 = node1.Left + (node1.Width / 2);
                X2 = node2.Left + (node2.Width / 2);
                Y1 = node1.Top +  (node1.Height / 2);
                Y2 = node2.Top + (node2.Height / 2);
            }


            
        }

    }
    
}
