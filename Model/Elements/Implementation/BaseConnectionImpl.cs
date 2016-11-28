﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Interface;

namespace Model.Elements.Implementation
{
    [Serializable]
    public class BaseConnectionImpl : BaseElementImpl, IBaseConnection
    {
        public BaseConnectionImpl() { }
        public IBaseNode node1 { get; }
        public IBaseNode node2 { get; }
        public double Left2 { get; set; }
        public double Top2 { get; set; }
        public string Color { get; set; }
        public BaseConnectionImpl(IBaseNode node1, IBaseNode node2)
        {
            this.Left = 1;
            this.Top = 1;
            this.Left2 = 101;
            this.Top2 = 51;

            /*
             this.Left = node1.Left;
            this.Top = node1.Top;
            this.Left2 = node2.Left;
            this.Top2 = node2.Top;
             */

            this.node1 = node1;
            this.node2 = node2;
            this.Color = "Yellow";
        }
    }
    
}
