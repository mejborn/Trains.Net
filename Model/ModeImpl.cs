using Model;
using Model.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrainsModel
{
    public class ModelImpl : IModel
    {
        List<IBaseElement> Elements;
        public ModelImpl()
        {
            Elements = new List<IBaseElement>();
            AddElement(new BaseStationImpl("First"));
        }

        public void addNode()
        {
            IBaseNode Node = new BaseNodeImpl()
            {
                Left = 100,
                Top = 100,
            };
            AddElement(Node);
        }
        public void addStation()
        {
            IBaseStation station = new BaseStationImpl("Second")
            {
                Left = 50,
                Top = 50,
                Color = "Green",
            };
            AddElement(station);

            IBaseConnection connection = new BaseConnectionImpl()
            {
                Left = Elements[0].Left,
                Top = Elements[0].Top,
                Left2 = station.Left,
                Top2 = station.Top,
            };
            AddElement(connection);


        }

        private void AddElement(IBaseElement Element)
        {
            Element.Id = Elements.Count + 1;
            Elements.Add(Element);
        }

        public List<IBaseElement> GetElements()
        {
            return Elements;
        }
        
    }
}
