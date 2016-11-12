using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class BaseConnectionImpl : BaseElementImpl, IBaseConnection
    {
        public List<IBaseConnection> Connections { get; }
        public string Color { get; set; }
        public int Left2 { get; set; }
        public int Top2 { get; set; }
        public BaseConnectionImpl()
        {
            this.Left = 0;
            this.Top = 0;
            this.Left2 = 200;
            this.Top2 = 200;
            this.Color = "Yellow";
        }
    }
    
}
