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

            IBaseStation st1 = new BaseStationImpl("1",20,20);
            IBaseStation st2 = new BaseStationImpl("2", 30, 30);
            IBaseStation st3 = new BaseStationImpl("3", 40, 40);
            IBaseStation st4 = new BaseStationImpl("4", 50, 50);

            IBaseNode no1 = new BaseNodeImpl(60,60);

            ConnectNodes(st1,st2);
            ConnectNodes(st3, st1);
            ConnectNodes(st1, no1);
            ConnectNodes(no1, st4);

            Console.WriteLine("FIIIIIIIIIIIIIISK");

            Console.WriteLine(st1.Connections.Count);
            //Console.WriteLine(GetStationsConnectedToNode(st1).Count);
            //Console.WriteLine(GetNodesConnectedToNode(st1).Count);

            foreach (var hej in GetStationsConnectedToNode(st1))
            {
                Console.WriteLine(hej.Left);
                Console.WriteLine(hej.Name);
            }

            Console.WriteLine("-----------------");
            Console.WriteLine("FIIIIIIIIIIIIIISK");

            foreach (var hej in GetNodesConnectedToNode(st1))
            {
                Console.WriteLine(hej.Left);
            }

            AddElement(Node);
            AddElement(st1);
            AddElement(st2);
            AddElement(st3);
            AddElement(st4);
            AddElement(no1);
        }
        public void AddStation(string name, double left, double top)
        {
            
           
            for(int i = 0; i < 100; i++)
            {
                IBaseStation station = new BaseStationImpl(i.ToString(), left, top);
                AddElement(station);
            }

            
        }

        public void RemoveElement(IBaseElement element)
        {
            Elements.Remove(element);
        }

        public void ConnectNodes(IBaseNode node1, IBaseNode node2) 
        {
            foreach (var node1Connection in node1.Connections) // Take one of the notes and checks for existing connection between the two
            {
                if ((node1Connection.node1 == node1 && node1Connection.node2 == node2)
                    || (node1Connection.node1 == node2 && node1Connection.node2 == node1))
                    throw new Exception("The given connection already exists");
            }
            
            IBaseConnection connection = new BaseConnectionImpl(node1, node2);
            node1.AddConnection(connection);
            node2.AddConnection(connection);

            Elements.Add(connection);

        }

        private List<IBaseStation> AuxiliaryGetStationsConnectedToNode(IBaseNode node, List<IBaseNode> parents)
        {
            List<IBaseStation> connectedStations = new List<IBaseStation>();
            List<IBaseNode> connectedNotes = GetNodesConnectedToNode(node);

            foreach (var searchNode in connectedNotes)
            {
                if (parents.Contains(searchNode))
                {
                    Console.WriteLine("Got same parent:" + searchNode.Left);
                    continue;
                }

                if (!(searchNode is IBaseStation))
                {
                    Console.WriteLine("This element is NOT station" + searchNode.Left);
                    parents.Add(node);
                    return connectedStations.Union(AuxiliaryGetStationsConnectedToNode(searchNode, parents)).ToList();
                }
                Console.WriteLine("Now writes for station:" + searchNode.Left);
                connectedStations.Add((IBaseStation)searchNode);
            }

            return connectedStations;
        }

        public List<IBaseStation> GetStationsConnectedToNode(IBaseNode node)
        {
            return AuxiliaryGetStationsConnectedToNode(node, new List<IBaseNode>());
        }

        public List<IBaseNode> GetNodesConnectedToNode(IBaseNode node)
        {
            List<IBaseConnection> nodeConnections = node.Connections;
            List<IBaseNode> foundNodes = new List<IBaseNode>();

            foreach (var connection in nodeConnections)
            {
                if (connection.node1 == node)
                {
                    foundNodes.Add(connection.node2);
                    continue;
                }
                foundNodes.Add(connection.node1);
            }

            return foundNodes;
        }

        public List<IBaseConnection> GetConnectionsToNode(IBaseNode node)
        {
            return node.Connections;
        }

        public void CopyNode(IBaseNode node)
        {
            AddNode(node.Left, node.Top);
        }

        public void CopyStation(string newName, IBaseStation station)
        {
            if (newName == station.Name) throw new Exception("The given name already exists");

            AddStation(newName, station.Left, station.Top);

        }


        private void AddElement(IBaseElement element)
        {
            Elements.Add(element);
        }

        public List<IBaseElement> GetElements()
        {
           return Elements;
        }

        
        
    }
}
