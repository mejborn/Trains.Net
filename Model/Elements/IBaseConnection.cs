using System.Collections.Generic;

namespace Model.Elements
{
    public interface IBaseConnection : IBaseElement
    {

        Elements.IBaseNode node1 { get; }
        Elements.IBaseNode node2 { get; }

        double Left2 { get; set; }
        double Top2 { get; set; }
    }
}
