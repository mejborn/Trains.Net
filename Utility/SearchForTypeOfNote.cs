using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class SearchForTypeOfNote
    {
        /*public static T AuxiliaryGetStationsConnectedToNode<T>(T node, List<T> parents, T searchCriteria)
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
        }*/



    }
}
