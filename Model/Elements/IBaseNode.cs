﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public interface IBaseNode : IBaseElement
    {
        string Color { get; set; }
        List<IBaseConnection> Connections { get; }
        bool AddConnection(IBaseConnection connection);
    }
}
