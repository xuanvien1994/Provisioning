using System;
using System.Collections.Generic;
using System.Text;

namespace MS.SCIM.Service.Monitor
{
    public interface INotification<TPayload>
    {
        long? Identifier { get; }
        string CorrelationIdentifier { get; }
        TPayload Message { get; }
    }
}
