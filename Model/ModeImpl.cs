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
        private List<IBaseElement> Elements { get; }

        public ModelImpl()
        {
            Elements = new List<IBaseElement>();
            AddElement(new BaseStationImpl("First", 10, 10));
        }

        public void AddNode(double left, double top)
        {
            IBaseNode Node = new BaseNodeImpl(left, top);
            
            AddElement(Node);
        }
        public void AddStation(string name, double left, double top)
        {
            IBaseStation station = new BaseStationImpl("Second", left, top);
           
            AddElement(station);
        }

        public void RemoveElement(IBaseElement element)
        {
            Elements.Remove(element);
        }

        public bool ConnectNodes(IBaseNode node1, IBaseNode node2)
        {
            IBaseConnection connection = new BaseConnectionImpl(node1, node2);

            bool isConnectionSucces = node1.AddConnection(connection) && node2.AddConnection(connection);
            
            Elements.Add(connection);

            return isConnectionSucces;

        }

        public List<IBaseNode> GetStationsConnectedToNode(IBaseNode node)
        {
            throw new NotImplementedException();
        }

        public List<IBaseNode> GetNodesConnectedToNode(IBaseNode node)
        {
            throw new NotImplementedException();
        }

        public List<IBaseConnection> GetConnectionsToNode(IBaseNode node)
        {
            throw new NotImplementedException();
        }

        public void CopyNode(IBaseNode node)
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
