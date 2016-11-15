﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class BaseNodeImpl : BaseElementImpl, IBaseNode
    {
        public string Color { get; set; }
        public List<IBaseElement> Connections { get; }

        public BaseNodeImpl()
        {   
            Connections = new List<IBaseElement>();
            Color = "Cyan";
        }

        public bool AddConnection(IBaseConnection connection)
        {
            if (!Connections.Contains(connection))
            {
                if (Connections.Count < 3)
                {
                    Connections.Add(connection);
                    return true;
                }
                else
                {
                    return false;
                    // ÉRROR: Not more than 3 allowed
                }

            }
            else
            {
                return false;
                // ERROR: Already exists
            }
        }

       
    }
}
