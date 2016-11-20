﻿using Model;
using Model.Elements;
using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace TrainsModel
{
    public class ModelImpl : IModel
    {
        List<IBaseElement> Elements;

        public String inputText { get; private set; }

        public ModelImpl()
        {
            Elements = new List<IBaseElement>();
            AddElement(new BaseStationImpl("First"));
        }

        public void AddNode()
        {
            IBaseNode Node = new BaseNodeImpl()
            {
                Left = 100,
                Top = 100,
            };
            AddElement(Node);
        }
        public void AddStation()
        {
            IBaseStation station = new BaseStationImpl("Second")
            {
                Left = 50,
                Top = 50,
                Color = "Green",
            };
            AddElement(station);
            ConnectNodes((IBaseNode)Elements[0], station);
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

        public void AddNode(double left, double top)
        {
            throw new NotImplementedException();
        }

        public void AddStation(string name, double left, double top)
        {
            throw new NotImplementedException;

        }
    }
}
