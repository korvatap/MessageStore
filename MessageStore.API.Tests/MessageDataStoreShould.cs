using MessageStore.API.Models;
using MessageStore.API.Storage;
using Xunit;

namespace MessageStore.API.Tests
{
    public class MessageDateStoreShould
    {
        [Fact]
        public void BeEmptyWhenNew()
        {
            var storage = new MessageDataStore();
            Assert.Empty(storage.GetMessages());
        }

        [Fact]
        public void ContainMessagesAfterAdding()
        {
            var storage = new MessageDataStore();
            
            var message = new Message
            {
                Title = "TestMessageTitle",
                Body = "TestMessageBody"
            };

            storage.AddMessage(message);

            var messageFromStore = storage.GetMessage(1);

            Assert.NotNull(messageFromStore);
        }

        [Fact]
        public void HaveSameNumberOfMessages()
        {
            var storage = new MessageDataStore();
            int numberOfMessages = 100;
            for (int i = 0; i < numberOfMessages; i++)
            {
                var message = new Message
                {
                    Title = $"TestMessageTitle{i}",
                    Body = $"TestMessageBody{i}"
                };

                storage.AddMessage(message);
            }

            Assert.Equal(numberOfMessages, storage.GetNumberOfMessages());
        }

        [Fact]
        public void NotContainMessageAfterRemove()
        {
            var storage = new MessageDataStore();

            var message = new Message
            {
                Title = "TestMessageTitle",
                Body = "TestMessageBody"
            };

            storage.AddMessage(message);
            storage.RemoveMessage(message);

            Assert.Equal(0, storage.GetNumberOfMessages());
        }
    }
}
