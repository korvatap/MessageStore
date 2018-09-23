using MessageStore.API.Models;
using MessageStore.API.Storage;
using Xunit;

namespace MessageStore.API.Tests
{
    public class MessageDataStoreShould : IClassFixture<TestFixture>
    {
        private IMessageDataStore _messageDataStore;

        public MessageDataStoreShould(TestFixture fixture)
        {
            _messageDataStore = fixture.DataStore;
        }

        [Fact]
        public void BeEmptyWhenNew()
        {
            Assert.Empty(_messageDataStore.GetMessages());
        }

        [Fact]
        public void ContainMessagesAfterAdding()
        {
            var message = new Message
            {
                Title = "TestMessageTitle",
                Body = "TestMessageBody"
            };

            _messageDataStore.AddMessage(message);

            var messageFromStore = _messageDataStore.GetMessage(1);

            Assert.NotNull(messageFromStore);
        }

        [Fact]
        public void HaveSameNumberOfMessages()
        {
            int numberOfMessages = 100;
            for (int i = 0; i < numberOfMessages; i++)
            {
                var message = new Message
                {
                    Title = $"TestMessageTitle{i}",
                    Body = $"TestMessageBody{i}"
                };

                _messageDataStore.AddMessage(message);
            }

            Assert.Equal(numberOfMessages, _messageDataStore.GetNumberOfMessages());
        }

        [Fact]
        public void NotContainMessageAfterRemove()
        {
            var message = new Message
            {
                Title = "TestMessageTitle",
                Body = "TestMessageBody"
            };

            _messageDataStore.AddMessage(message);
            _messageDataStore.RemoveMessage(message);

            Assert.Equal(0, _messageDataStore.GetNumberOfMessages());
        }
    }
}
