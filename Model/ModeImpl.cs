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

        public ModelImpl() { }

        public void AddNode(double left, double top)
        {
            var node = new BaseNodeImpl(left, top);
            AddElement(node);
            
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

        public void ConnectNodes(IBaseNode node1, IBaseNode node2) 
        {
            if (node1.Connections.Any(node1Connection => (node1Connection.node1 == node1 && node1Connection.node2 == node2)
                                                         || (node1Connection.node1 == node2 && node1Connection.node2 == node1)))
            {
                throw new Exception("The given connection already exists");
            }
            
            var connection = new BaseConnectionImpl(node1, node2);
            node1.AddConnection(connection);
            node2.AddConnection(connection);

            Elements.Add(connection);

            if (!(node1 is StationViewModel) || !(node2 is StationViewModel)) return; //Skal måske ændres ift. at noder laves automatisk ved connection af 2 station
            node1.Color = "Green";
            node2.Color = "Green";
        }

        public void DeleteConnection(BaseConnectionImpl connection)
        {
            var node1 = connection.node1;
            var node2 = connection.node2;
            node1.Connections.Remove(connection);
            node2.Connections.Remove(connection);
            Elements.Remove(connection);

            if (GetStationsConnectedToNode(node1).Count == 0) node1.Color = "Red";
            if (GetStationsConnectedToNode(node2).Count == 0) node2.Color = "Red";


        }

        private List<StationViewModel> AuxiliaryGetStationsConnectedToNode(IBaseNode node, List<IBaseNode> parents)
        {
            List<StationViewModel> connectedStations = new List<StationViewModel>();
            List<IBaseNode> connectedNotes = GetNodesConnectedToNode(node);

            foreach (var searchNode in connectedNotes)
            {
                if (parents.Contains(searchNode))
                {
                    continue;
                }

                if (!(searchNode is StationViewModel))
                {
                    parents.Add(node);
                    return connectedStations.Union(AuxiliaryGetStationsConnectedToNode(searchNode, parents)).ToList();
                }
                connectedStations.Add((StationViewModel)searchNode);
            }

            return connectedStations;
        }

        public List<StationViewModel> GetStationsConnectedToNode(IBaseNode node)
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

        public void CopyStation(string newName, StationViewModel station)
        {
            if (newName == station.Name) throw new Exception("The given name already exists");

            AddStation(newName, station.Left, station.Top);

        }

        public void DeleteStation(StationImpl station, bool exceptionThrown)
        {
            if (station.Connections.Any() && !exceptionThrown)
            {
                throw new Exception("This station is connected to other nodes/station. Are you sure, that you want to delete it?");
            }

            // Reverse for-loop used to compensate for modifying list while iterating over it
            for (int i = station.Connections.Count-1; i >= 0; i--)
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

        private void AddElement(BaseElementImpl element)
        {
            Elements.Add(element);
        }

        public List<BaseElementImpl> GetElements()
        {
           return Elements;
        }

        public void StationInfo(double left, double top)
        {
            StationInfoImpl info = new StationInfoImpl(left, top);
            Elements.Add(info);
            info.show();
        }
    }
}
