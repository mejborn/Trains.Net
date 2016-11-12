using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public abstract class BaseElementImpl : IBaseElement
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Id { get; set; }
    }
}
