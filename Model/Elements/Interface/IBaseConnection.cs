using System.Collections.Generic;

namespace Model.Elements.Interface
{
    public interface IBaseConnection : IBaseElement
    {

        IBaseNode node1 { get; }
        IBaseNode node2 { get; }
        double X1{ get; set; }
        double X2 { get; set; }
        double Y1 { get; set; }
        double Y2 { get; set; }
        string Color { get; set; }
    }
}
