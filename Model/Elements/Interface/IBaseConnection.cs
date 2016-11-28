using System.Collections.Generic;

namespace Model.Elements.Interface
{
    public interface IBaseConnection : IBaseElement
    {

        IBaseNode node1 { get; }
        IBaseNode node2 { get; }

        double Left2 { get; set; }
        double Top2 { get; set; }
    }
}
