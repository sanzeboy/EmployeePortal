using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePortal.Application.Events
{
    public interface IEventSubscriber<TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}
