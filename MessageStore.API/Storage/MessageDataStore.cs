using System.Collections.Generic;
using MessageStore.API.Models;

namespace MessageStore.API.Storage
{
    public class MessageDataStore : IMessageDataStore
    {
        private List<Message> Messages { get; set; } = new List<Message>();

        public MessageDataStore()
        {
            for(int i = 0; i < 100; i++) {
                var message = new Message 
                {
                    Id = i,
                    Title = $"testTitle{i}",
                    Body = $"testBody{i}"
                };
                Messages.Add(message);
            }
        }

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
            Messages.Add(message);
        }

        public void RemoveMessage(Message message)
        {
            Messages.Remove(message);
        }
    }
}