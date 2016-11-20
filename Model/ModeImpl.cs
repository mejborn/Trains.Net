﻿using Model;
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
            AddElement(new StationImpl("First", 10, 10));
        }

        public void AddNode(double left, double top)
        {
            IBaseNode Node = new BaseNodeImpl(left, top);
            AddElement(Node);
            
        }
        public void AddStation(string name, double left, double top)
        {
            IStation station = new StationImpl(name, left, top);
            AddElement(station);
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
                    return connectedStations.Union(AuxiliaryGetStationsConnectedToNode(searchNode, parents)).ToList();
                }
                connectedStations.Add((IStation)searchNode);
            }

            return connectedStations;
        }

        public List<IStation> GetStationsConnectedToNode(IBaseNode node)
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

        public void CopyStation(string newName, IStation station)
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
