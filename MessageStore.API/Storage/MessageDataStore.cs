using System.Collections.Generic;
using MessageStore.API.Models;

namespace MessageStore.API.Storage
{
    public class MessageDataStore
    {
        public static MessageDataStore Current { get; } = new MessageDataStore();

        public List<Message> Messages { get; set; } = new List<Message>();

        public int NumberOfMessages
        {
            get
            {
                return Messages.Count;
            }
        }
    }
}