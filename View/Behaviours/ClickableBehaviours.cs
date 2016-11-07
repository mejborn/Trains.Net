using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Behaviours
{
    class ClickableBehaviours
    {

        public bool PassEventArgsOnClick
        {
            get { return (bool) GetValue(PassEventArgsOnClickProperty); }
            set { SetValue(PassEventArgsOnClickProperty, value); }
        }
    }
}
