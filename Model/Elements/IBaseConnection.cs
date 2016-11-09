using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public interface IBaseConnection : IBaseElement
    {
        int Left2 { get; set; }
        int Top2 { get; set; }
    }
}
