using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Elements
{
    public interface IBaseElement
    {
        double Left { get; set; }
        double Top { get; set; }
        int Width { get; set; }
        int Height { get; set; }

    }
}
