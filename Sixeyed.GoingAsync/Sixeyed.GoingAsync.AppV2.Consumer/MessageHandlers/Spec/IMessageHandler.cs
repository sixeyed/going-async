using Sixeyed.GoingAsync.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.AppV2.Consumer.MessageHandlers
{
    public interface IMessageHandler
    {
        bool IsHandled(string messageType);

        void Handle(IMessage message);
    }
}
