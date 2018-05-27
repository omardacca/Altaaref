using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Helpers
{
    public interface IFCMNotificationSubscriber
    {
        void Subscribe(string topic);
        void UnSubscribe(string topic);
    }
}
