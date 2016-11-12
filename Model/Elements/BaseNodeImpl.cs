using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class BaseNodeImpl : BaseElementImpl, IBaseNode
    {
        public string Color { get; set; }
        public ObservableCollection<IBaseElement> Connections { get; }

        public BaseNodeImpl()
        {
            Color = "Cyan";
        }
    }
}
