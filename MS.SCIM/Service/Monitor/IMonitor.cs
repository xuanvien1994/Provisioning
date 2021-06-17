using System;
using System.Collections.Generic;
using System.Text;

namespace MS.SCIM.Service.Monitor
{
    public interface IMonitor
    {
        void Inform(IInformationNotification notification);
        //void Report(IExceptionNotification notification);
        //void Warn(Notification<Exception> notification);
        //void Warn(Notification<string> notification);
    }
}
