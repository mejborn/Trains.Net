using Model;
using Model.Elements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        public void removeStaion(IBaseNode node)
        {
            throw new NotImplementedException();
        }

        public void connectNodes(IBaseNode node1, IBaseNode node2)
        {
            throw new NotImplementedException();
        }

        public List<IBaseNode> getStationsConnectedToNode(IBaseNode node)
        {
            throw new NotImplementedException();
        }

        public List<IBaseNode> getNodesConnectedToNode(IBaseNode node)
        {
            throw new NotImplementedException();
        }

        public List<IBaseConnection> getConnectionsToNode(IBaseNode node)
        {
            throw new NotImplementedException();
        }

        public IBaseNode getOtherNodeFromConnection(IBaseConnection connect, IBaseNode callerNode)
        {
            throw new NotImplementedException();
        }

        private void AddElement(IBaseElement Element)
        {
            Elements.Add(Element);
        }

        public List<IBaseElement> GetElements()
        {
           return Elements;
        }

        
        
    }
}
