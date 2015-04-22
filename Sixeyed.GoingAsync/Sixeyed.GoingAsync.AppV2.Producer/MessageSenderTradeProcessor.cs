using Sixeyed.GoingAsync.Core.Incoming;
using Sixeyed.GoingAsync.Core.Messages;
using Sixeyed.GoingAsync.Core.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;

namespace Sixeyed.GoingAsync.AppV2.Producer
{
    public class MessageSenderTradeProcessor : IIncomingTradeProcessor
    {
        private readonly ITradeContextFactory _dbFactory;
        private Dictionary<string, MessageQueue> _messageQueues;

        public MessageSenderTradeProcessor(ITradeContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
            _messageQueues = new Dictionary<string, MessageQueue>();
            foreach (var queueKey in ConfigurationManager.AppSettings.AllKeys.Where(x=>x.StartsWith("Queue.")))
            {
                _messageQueues[queueKey] = new MessageQueue(ConfigurationManager.AppSettings[queueKey], QueueAccessMode.Send);
            }
        }

        public void Process(IncomingTrade incomingTrade)
        {
            Send(new ValidateTradeMessage
            {
                TradeId = incomingTrade.Id
            });

            Send(new EnrichParty1Message
            {
                TradeId = incomingTrade.Id,
                Party1Lei = incomingTrade.Party1Lei
            });

            Send(new EnrichParty2Message
            {
                TradeId = incomingTrade.Id,
                Party2Lei = incomingTrade.Party2Lei
            });

            Console.WriteLine("* Sent messages to processed trade with ID: {0}", incomingTrade.Id);
        }

        private void Send(object message)
        {
            var messageType= message.GetMessageTypeName();
            var messageKey = "Queue." + messageType;
            var queue = _messageQueues[messageKey];            

            using (var messageStream = message.ToJsonStream())
            {
                var msg = new Message();
                msg.BodyStream = messageStream;
                msg.Label = messageType;

                queue.Send(msg);
            }
        }

        public void Dispose()
        {
            foreach (var queue in _messageQueues.Values)
            {
                queue.Close();
                queue.Dispose();
            }
            _messageQueues.Clear();
        }
    }
}
