using System;
using System.Collections.Generic;
using System.Linq;
using MessageStore.API.Models;

namespace MessageStore.API.Storage
{
    public class MessageDataStore : IMessageDataStore
    {
        private List<Message> Messages { get; set; } = new List<Message>();

        public List<Message> GetMessages()
        {
            return Messages;
        }

        public int GetNumberOfMessages()
        {
            return Messages.Count;
        }

        public void AddMessage(Message message)
        {
            message.Id = (GetNumberOfMessages()) + 1;
            message.CreatedAt = DateTime.Now;
            message.ModifiedAt = DateTime.Now;
            Messages.Add(message);
        }

        public void RemoveMessage(Message message)
        {
            Messages.Remove(message);
        }

        public Message GetMessage(int id)
        {
            return Messages.FirstOrDefault(c => c.Id == id);
        }
    }
}