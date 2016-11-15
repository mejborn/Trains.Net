using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public abstract class BaseElementImpl : IBaseElement
    {
        public double Left { get; set; }
        public double Top { get; set; }
    }
}
