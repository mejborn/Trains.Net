using Model.Elements;

namespace ViewModel
{
    public class BaseNodeViewModel : BaseElementViewModel
    {
        public string Color { get; set; }
        public BaseNodeViewModel(IBaseNode Element) : base(Element)
        {
            Color = Element.Color;
        }
        public void UpdateConnections()
        {
            // Update Top and Left of connections
        }
    }
}
