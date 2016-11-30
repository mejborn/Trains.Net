using Model.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Implementation;
using Model.Elements.Interface;

namespace Model
{
    public interface IModel
    {
        List<BaseElementImpl> GetElements();
        BaseNodeImpl AddNode(double left, double top);
        StationImpl AddStation(string name, double left, double top);

        void RemoveElement(BaseElementImpl element);

        BaseConnectionImpl ConnectNodes(IBaseNode node1, IBaseNode node2, ConnectionPointImpl cp1, ConnectionPointImpl cp2); // Create new BaseConnectionImpl

        void RemoveConnection(BaseConnectionImpl connection); 

        StationInfoImpl StationInfo(IStation station);

        List<IStation> GetStationsConnectedToNode(IBaseNode node); // This method may use the three following methods (here, we only want stations from node/station)

        List<IBaseNode> GetNodesConnectedToNode(IBaseNode node); // This method may use the two following methods (here, node can be both node and station)

        List<BaseConnectionImpl> GetConnectionsToNode(IBaseNode node);

        void CopyNode(IBaseNode node); //Copy-paste functionality - should maybe be in Utils?
        void CopyStation(string newName, IStation station);

        void DeleteStation(StationImpl station);
        void DeleteNode(BaseNodeImpl node);
        void DeleteConnection(BaseConnectionImpl connection);
        void DeleteConnectionPoint(ConnectionPointImpl cp);
        void DeleteObject(object o);
        void AddElement(BaseElementImpl element);
    }
}
