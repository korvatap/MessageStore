using System;
using MessageStore.API.Controllers;
using MessageStore.API.Models;
using MessageStore.API.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace MessageStore.API.Tests
{
    public class TestFixture
    {
        public TestFixture()
        {
            var services = new ServiceCollection();
            services.AddTransient<IMessageDataStore, MessageDataStore>();
            services.AddTransient<MessageController, MessageController>((ctx) =>
            {
                var dataStore = ctx.GetService<IMessageDataStore>();

                for (int i = 0; i < NumberOfMessages; i++)
                {
                    var message = new Message
                    {
                        Title = $"TestMessageTitle{i}",
                        Body = $"TestMessageBody{i}"
                    };

                    dataStore.AddMessage(message);
                }

                return new MessageController(dataStore);
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        private IServiceProvider ServiceProvider { get; set; }

        public int NumberOfMessages { get; } = 100;

        public IMessageDataStore DataStore => ServiceProvider.GetService<IMessageDataStore>();

        public MessageController MessageController => ServiceProvider.GetService<MessageController>();
    }
}
