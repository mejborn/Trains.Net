﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Model.Elements.Interface;

namespace Model.Elements.Implementation
{
    [XmlType("Node")]
    public class BaseNodeImpl : BaseElementImpl, IBaseNode
    {
        public BaseNodeImpl() { }
        public string Color { get; set; }
        public double Opacity { get; set; } = 1;
        [XmlIgnore]
        public List<BaseConnectionImpl> Connections { get; } = new List<BaseConnectionImpl>();
        [XmlArray("NodesConnected"), XmlArrayItem("Node")]
        public List<BaseNodeImpl> NodesConnected { get; }
        [XmlArray("StationsConnected"), XmlArrayItem("Station")]
        public List<StationImpl> StationsConnected { get; }

        public BaseNodeImpl(double left, double top)
        {
            Width = 20;
            Height = 20;
            Left = left;
            Top = top;
            Color = "Brown";
            
        }

        public void AddConnection(BaseConnectionImpl connection)
        {
            if (Connections.Count >= 3) throw new Exception("A node cannot have more than 3 connecions!");

            Connections.Add(connection);
            
        }
    }
}
