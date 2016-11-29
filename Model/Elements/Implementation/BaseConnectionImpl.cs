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
        public BaseConnectionImpl(IBaseNode node1, IBaseNode node2)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.Left = 0;
            this.Top = 0;
            CalculatePosision();

            /*
             this.Left = node1.Left;
            this.Top = node1.Top;
            this.Left2 = node2.Left;
            this.Top2 = node2.Top;
             */
             
            Color = "Yellow";

           
        }

        public void CalculatePosision()
        {

            X1 = node1.Left + (node1.Width/2);
            X2 = node2.Left + (node2.Width/2);
            Y1 = node1.Top + (node1.Height / 2);
            Y2 = node2.Top + (node2.Height / 2);




            // int 

            //node1.

        }

    }
    
}
