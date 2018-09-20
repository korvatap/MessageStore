using System;
using MessageStore.API.Storage;
using Xunit;

namespace MessageStore.API.Tests
{
    public class MessageControllerShould
    {
        [Fact]
        public void BeEmptyWhenNew()
        {
            var storage = new MessageDataStore();
            Assert.Empty(storage.Messages);
        }
    }
}
