using System.Collections.Generic;
using MessageStore.API.Models;

namespace MessageStore.API.Storage
{
    public class MessageDataStore
    {
        public static MessageDataStore Current { get; } = new MessageDataStore();

        public List<Message> Messages { get; set; } = new List<Message>();

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

        public int NumberOfMessages
        {
            get
            {
                return Messages.Count;
            }
        }
    }
}