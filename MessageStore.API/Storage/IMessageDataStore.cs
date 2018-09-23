using System.Collections.Generic;
using MessageStore.API.Models;

namespace MessageStore.API.Storage
{
    public interface IMessageDataStore
    {
        List<Message> GetMessages();
        
        int GetNumberOfMessages();

        void AddMessage(Message message);

        void RemoveMessage(Message message);

        Message GetMessage(int id);
    }
}