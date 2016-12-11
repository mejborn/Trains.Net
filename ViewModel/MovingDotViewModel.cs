using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Model.Elements.Interface;

namespace ViewModel
{
    public class MovingDotViewModel : ConnectionPointViewModel
    {
        private Point _startPoint, _endPoint;
        private bool _turned;
        private new string Color { get; set; }
        public MovingDotViewModel(Point startPoint, Point endPoint, IBaseElement element) : base(element)
        {
            Color = "Blue";
            _startPoint = startPoint;
            _endPoint = endPoint;
            Top = _startPoint.Y;
            Left = _startPoint.X;
            var thread = new Thread(MoveDot);
            thread.Start();
        }
        public MovingDotViewModel(IBaseElement element) : base(element)
        {
            
        }

        public void MoveDot()
        {
            while (true)
            {
                if (!_turned)
                {
                    Top += (Top - _endPoint.Y)/10000;
                    Left += (Left - _endPoint.X)/10000;
                }
                else
                {
                    //Top -= (_endPoint.Y - _startPoint.Y) / 10000;
                    //Left -= (_endPoint.X - _startPoint.X) / 10000;
                }
                if (Math.Abs(Top - _startPoint.Y) < 0.001 || Math.Abs(Top - _endPoint.Y) < 0.001)
                    _turned = !_turned;
                Thread.Sleep(10);
            }
        }
    }
}
