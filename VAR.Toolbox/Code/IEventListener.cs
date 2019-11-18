using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAR.Toolbox.Code
{
    public interface IEventListener
    {
        void ProcessEvent(string eventName, object eventData);
    }
}
