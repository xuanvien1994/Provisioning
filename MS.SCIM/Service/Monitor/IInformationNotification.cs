using System;
using System.Collections.Generic;
using System.Text;

namespace MS.SCIM.Service.Monitor
{
    public interface IInformationNotification : INotification<string>
    {
        bool Verbose { get; set; }
    }
}
