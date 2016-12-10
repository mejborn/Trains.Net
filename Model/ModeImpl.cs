using Model;
using Model.Elements;
using Model.Elements.Implementation;
using Model.Elements.Interface;
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
using System.Xml.Serialization;

namespace TrainsModel
{
    [XmlRoot]
    public class ModelImpl : IModel
    {
        [XmlArray("Elements"), XmlArrayItem("Station")]
        public List<BaseElementImpl> Elements { get; } = new List<BaseElementImpl>();

        /*
         * Should save the lines in some way 
         */
        public Dictionary<String, List<IStation>> Lines { get; } = new Dictionary<string, List<IStation>>();

        public ModelImpl() { }

        public BaseNodeImpl AddNode(double left, double top)
        {
            var node = new BaseNodeImpl(left, top);
            AddElement(node);
            return node;
        }
        public StationImpl AddStation(string name, double left, double top)
        {
            int allowedLenght = 12;
            if (String.IsNullOrWhiteSpace(name)) throw new Exception("You must specify a name for the station!");
            if (name.Length > allowedLenght) throw new Exception("The name must not be greater than " + allowedLenght + " characters long");

            foreach (var element in Elements)
            {
                if (element is StationImpl && ((StationImpl)element).Name == name)
                {
                    throw new Exception("A station with the given name already exists!");
                }
                
            }
                //GetElements().Any(v => v is StationImpl)

            var station = new StationImpl(name, left, top);
            AddElement(station);

            //UndoAndRedoController.instanceOfUndoRedo.AddToStackAndExecute(new AddStationCommand(Elements,station));
            //Console.WriteLine("Model");
            return station;

        }

        public void RemoveElement(BaseElementImpl element)
        {
            Elements.Remove(element);
        }

        public BaseConnectionImpl ConnectNodes(IBaseNode node1, IBaseNode node2, ConnectionPointImpl cp1, ConnectionPointImpl cp2) 
        {
            if (node1.Connections.Any(node1Connection => (node1Connection.node1 == node1 && node1Connection.node2 == node2)
                                                         || (node1Connection.node1 == node2 && node1Connection.node2 == node1)))
            {
                throw new Exception("The given connection already exists");
            }
            
            var connection = new BaseConnectionImpl(node1, node2, cp1, cp2);
            node1.AddConnection(connection);
            node2.AddConnection(connection);
            
            Elements.Add(connection);
            return connection;
        }

        public void DeleteConnection(BaseConnectionImpl connection)
        {
            var node1 = connection.node1;
            var node2 = connection.node2;

            if (node1 is StationImpl)
            {
               (node1 as StationImpl).ConnectionPoints.Remove(connection.CP1);
            }

            if (node2 is StationImpl)
            {
                (node2 as StationImpl).ConnectionPoints.Remove(connection.CP2);
            }
            
            node1.Connections.Remove(connection);
            node2.Connections.Remove(connection);
            Elements.Remove(connection);

        }

        private List<IStation> AuxiliaryGetStationsConnectedToNode(IBaseNode node, List<IBaseNode> parents)
        {
            List<IStation> connectedStations = new List<IStation>();
            List<IBaseNode> connectedNotes = GetNodesConnectedToNode(node);

            foreach (var searchNode in connectedNotes)
            {
                if (parents.Contains(searchNode))
                {
                    continue;
                }

                if (!(searchNode is IStation))
                {
                    parents.Add(node);
                    connectedStations = connectedStations.Union(AuxiliaryGetStationsConnectedToNode(searchNode, parents)).ToList();
                }
                else
                {
                    connectedStations.Add((IStation)searchNode);
                }
                
            }

            return connectedStations;
        }

        public List<IStation> GetStationsConnectedToNode(IBaseNode node)
        {
            return AuxiliaryGetStationsConnectedToNode(node, new List<IBaseNode>());
        }

        public List<IBaseNode> GetNodesConnectedToNode(IBaseNode node)
        {
            List<BaseConnectionImpl> nodeConnections = node.Connections;
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

        public List<BaseConnectionImpl> GetConnectionsToNode(IBaseNode node)
        {
            return node.Connections;
        }

        public void CopyNode(IBaseNode node)
        {
            AddNode(node.Left, node.Top);
        }

        public void CopyStation(string newName, IStation station)
        {
            if (newName == station.Name) throw new Exception("The given name already exists");
            AddStation(newName, station.Left, station.Top);

        }

        public void DeleteObject(object o)
        {
            if(o is StationImpl)
                DeleteStation(o as StationImpl);
            else if(o is BaseNodeImpl)
                DeleteNode(o as BaseNodeImpl);
        }

        public void DeleteStation(StationImpl station)
        {
            // Reverse for-loop used to compensate for modifying list while iterating over it
            for (var i = station.Connections.Count-1; i >= 0; i--)
            {
                DeleteConnection(station.Connections[i]);
            }
            Elements.Remove(station);
        }

        public void DeleteNode(BaseNodeImpl node)
        { //Not good to copy-paste from above, but can't get head around making it generic...need to pass node, Elements and make DeleteConnection generic too?
            for (int i = node.Connections.Count - 1; i >= 0; i--)
            {
                DeleteConnection(node.Connections[i]);
            }
            Elements.Remove(node);
        }

        
        public void DeleteConnectionPoint(ConnectionPointImpl cp)
        {
            throw new NotImplementedException();
        }

        public void AddElement(BaseElementImpl element)
        {
            Elements.Add(element);
        }

        public List<BaseElementImpl> GetElements()
        {
           return Elements;
        }
        /*
        public StationInfoImpl StationInfo(IStation Station)
        {
            try
            {
                Elements.Remove(Info);
            }
            catch (Exception)
            {
                Console.WriteLine("No information on display");
            }
            Info = new StationInfoImpl(Station);
            Elements.Add(Info);
            return Info;
        }*/

        public void RemoveConnection(BaseConnectionImpl connection)
        {
            throw new NotImplementedException();
        }

        public List<IStation> CreateLine(string name, IStation station1, IStation station2)
        {
            // Check if there no other lines with the given name
            if (Lines.ContainsKey(name)) return null;

            // Create line
            

            //if (!GetStationsConnectedToNode(station1).Contains(station2)) return null;
            Lines.Add(name, new List<IStation>());
            List<IStation> Stations = FindLine(name, station1, station2, new List<IBaseNode>());
            Stations.Add(station1);
            Lines[name] = Stations;

            return Lines[name];
        }

        public List<IStation> FindLine(String name, IBaseNode station1, IStation station2, List<IBaseNode> visited)
        {
            List<IStation> Stations = new List<IStation>();
            List<IBaseNode> ConnectedNodes = GetNodesConnectedToNode(station1);

            foreach (var node in ConnectedNodes)
            {
                if (visited != null && visited.Contains(node)) continue;
                if (node.Equals(station2))
                {
                    Stations.Add(station2);
                    return Stations;
                }

                if (!(node is IStation))
                {
                    visited.Add(node);
                    return FindLine(name, node, station2, visited);
                }
                visited.Add(node);
                Stations = FindLine(name, node, station2, visited);
                if (Stations != null)
                {
                    var station = node as IStation;
                    Stations.Add(station);
                    return Stations;
                }
            }
            return null;
        }

        public Dictionary<string, List<IStation>> GetLines()
        {
            return Lines;
        }
    }
}
