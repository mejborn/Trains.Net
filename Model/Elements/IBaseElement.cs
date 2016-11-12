using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public interface IBaseElement
    {
        int Left { get; set; }
        int Top { get; set; }
        int Id { get; set; }
    }
}
