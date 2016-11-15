

namespace Model.Elements
{
    public interface IBaseConnection : IBaseElement
    {
        List<IBaseNode> Nodes { get; }

        double Left2 { get; set; }
        double Top2 { get; set; }
    }
}
