using Model.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IModel
    {
        List<IBaseElement> GetElements();
        void AddNode(double left, double top);
        void AddStation(string name, double left, double top);

        void RemoveElement(IBaseElement element);

        bool ConnectNodes(IBaseNode node1, IBaseNode node2); // Create new BaseConnectionImpl

        List<IBaseNode> GetStationsConnectedToNode(IBaseNode node); // This method may use the three following methods (here, we only want stations from node/station)

        List<IBaseNode> GetNodesConnectedToNode(IBaseNode node); // This method may use the two following methods (here, node can be both node and station)

        List<IBaseConnection> GetConnectionsToNode(IBaseNode node);

        void CopyNode(IBaseNode node); //Copy-paste functionality - should maybe be in Utils?



        



    }
}
