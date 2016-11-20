using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class ConnectionPointImpl : BaseElementImpl,IConnectionPoint
    {
        public IBaseConnection Connection { get; set; }
        public IStation Station { get; set; }
        
        public ConnectionPointImpl() {}
    }
}
