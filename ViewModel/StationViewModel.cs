using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Model.Elements.Interface;

namespace ViewModel
{
    public class StationViewModel : NodeViewModel
    {
        public string Name { get; set; }
        public ObservableCollection<BaseElementViewModel> ConnectionPoints { get; } = new ObservableCollection<BaseElementViewModel>();

        public StationViewModel(IStation element) : base(element)
        {
            Name = element.Name;
            foreach (IConnectionPoint ConnectionPoint in element.ConnectionPoints) { ConnectionPoints.Add(Util.CreateViewModel(ConnectionPoint)); }
        }

        internal void AddConnectionPoint(string v)
        {
            IStation station = Element as IStation;
            station?.AddConnectionPoint(v);
            UpdateConnectionPointPositions();
        }

        private void UpdateConnectionPointPositions()
        {
            ConnectionPoints.Clear();
            IStation station = Element as IStation;
            IEnumerable<IConnectionPoint> LeftConnections = station?.ConnectionPoints.Where(p => p.AssociatedSide.Equals("Left"));
            IEnumerable<IConnectionPoint> RightConnections = station?.ConnectionPoints.Where(p => p.AssociatedSide.Equals("Right"));
            IEnumerable<IConnectionPoint> TopConnections = station?.ConnectionPoints.Where(p => p.AssociatedSide.Equals("Top"));
            IEnumerable<IConnectionPoint> BottomConnections = station?.ConnectionPoints.Where(p => p.AssociatedSide.Equals("Bottom"));
            UpdateConnectionPointsOnSide(LeftConnections); UpdateConnectionPointsOnSide(RightConnections);
            UpdateConnectionPointsOnSide(TopConnections); UpdateConnectionPointsOnSide(BottomConnections);
        }
        private void UpdateConnectionPointsOnSide(IEnumerable<IConnectionPoint> connectionPoints)
        {
            var sw = false; var cnt = 0;
            var baseElements = connectionPoints as IList<IConnectionPoint> ?? connectionPoints.ToList();
            foreach(var point in baseElements)
            {
                point.Top =
                     point.AssociatedSide.Equals("Top") ? -12:
                     point.AssociatedSide.Equals("Bottom") ? Element.Height -3 :
                     baseElements.Count() % 2 != 0 ?
                         sw ? Element.Height / 2 + point.Height * cnt -5 :
                         Element.Height / 2 - point.Height * cnt -5 :
                     sw ? Element.Height / 2 + point.Height / 2 + point.Height * cnt -5:
                     Element.Height / 2 - point.Height / 2 - point.Height * cnt -5;
                point.Left =
                    point.AssociatedSide.Equals("Left") ? -12 :
                    point.AssociatedSide.Equals("Right") ? Element.Width -3 :
                    baseElements.Count() % 2 != 0 ?
                        sw ? Element.Width / 2 + point.Width * cnt -5:
                        Element.Width / 2 - point.Width * cnt -5:
                    sw ? Element.Width / 2 + point.Width / 2 + point.Width * cnt -5:
                    Element.Width / 2 - point.Width / 2 - point.Width * cnt -5;
                cnt = baseElements.Count() % 2 == 0 ? 
                         sw ? cnt + 1 : cnt :
                      !sw ? cnt + 1 : cnt;
                sw = !sw;
                ConnectionPoints.Add(new ConnectionPointViewModel(point));
            }
        }
    }
}
