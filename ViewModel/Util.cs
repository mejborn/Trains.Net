using Model.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    static class Util
    {
        static Dictionary<Type, Type> TypeMap = new Dictionary<Type, Type>
        {
            {typeof(StationImpl), typeof(StationViewModel)},
            {typeof(BaseNodeImpl), typeof(BaseNodeViewModel)},
            {typeof(BaseConnectionImpl), typeof(BaseConnectionViewModel) },
            {typeof(ConnectionPointImpl),typeof(ConnectionPointViewModel) },
        };

        public static BaseElementViewModel CreateViewModel(IBaseElement Element)
        {
            return Activator.CreateInstance(TypeMap[Element.GetType()], Element) as BaseElementViewModel;
        }
    }
}
