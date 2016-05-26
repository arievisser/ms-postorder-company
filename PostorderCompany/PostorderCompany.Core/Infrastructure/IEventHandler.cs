using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostorderCompany.Core.Infrastructure
{
    interface IEventHandler
    {
        void Start();
        void Stop();
    }
}
