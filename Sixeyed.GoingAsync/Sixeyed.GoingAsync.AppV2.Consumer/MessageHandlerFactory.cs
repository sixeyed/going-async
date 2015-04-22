using Sixeyed.GoingAsync.AppV2.Consumer.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.AppV2.Consumer
{
    public class MessageHandlerFactory
    {
        private readonly IEnumerable<IMessageHandler> _messageHandlers;

        public MessageHandlerFactory(IMessageHandler[] messageHandlers)
        {
            _messageHandlers = messageHandlers;
        }

        public IEnumerable<IMessageHandler> GetHandlers(string messageType) 
        { 
            var handlers = new List<IMessageHandler>();
            foreach (var handler in _messageHandlers)
            {
                if (handler.IsHandled(messageType))
                {
                    handlers.Add(handler);
                }
            }
            return handlers;
        }
    }
}
