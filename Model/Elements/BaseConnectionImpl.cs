using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    class BaseConnectionImpl : IBaseConnection
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public string Color { get; set; }
        public List<IBaseConnection> Connections { get; }

        public int Left2
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Top2
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public BaseConnectionImpl()
        {
            this.Left = 100;
            this.Top = 100;
            this.Color = "Yellow";
        }
    }
    }
}
