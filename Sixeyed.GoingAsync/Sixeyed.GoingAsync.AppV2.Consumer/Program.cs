using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Sixeyed.GoingAsync.AppV2.Consumer.MessageHandlers;
using Sixeyed.GoingAsync.Core.Messages;
using Sixeyed.GoingAsync.Core.Model;
using System.IO;
using System.Linq;
using System.Messaging;

namespace Sixeyed.GoingAsync.AppV2.Consumer
{
    class Program
    {        
        private static MessageHandlerFactory _HandlerFactory;

        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterType<ITradeContextFactory, TradeContextFactory>();

            container.RegisterType<IMessageHandler, ValidateTradeHandler>("ValidateTradeHandler");
            container.RegisterType<IMessageHandler, EnrichPartyHandler>("EnrichParty1Handler",
                                                        new InjectionConstructor(
                                                                new ResolvedParameter<ITradeContextFactory>(),
                                                                true, false));
            container.RegisterType<IMessageHandler, EnrichPartyHandler>("EnrichParty2Handler",
                                                        new InjectionConstructor(
                                                                new ResolvedParameter<ITradeContextFactory>(),
                                                                false, true));            

            container.RegisterType<MessageHandlerFactory>();

            _HandlerFactory = container.Resolve<MessageHandlerFactory>();

            var arguments = Args.Configuration.Configure<Arguments>().CreateAndBind(args);
            var queue = new MessageQueue(arguments.InputQueue, QueueAccessMode.Receive);
            while (true)
            {
                var msg = queue.Receive();
                Handle(msg);
            }
        }

        private static void Handle(Message msg)
        {
            var messageType = msg.Label;
            var handlers = _HandlerFactory.GetHandlers(messageType);
            if (handlers.Any())
            {
                string json;
                using (var reader = new StreamReader(msg.BodyStream))
                {
                    json = reader.ReadToEnd();
                }
                var message = JsonConvert.DeserializeObject<ValidateTradeMessage>(json) as IMessage;
                foreach (var handler in handlers)
                {
                    handler.Handle(message);
                }
            }
        }
    }
}
