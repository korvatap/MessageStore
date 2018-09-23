using System;

namespace MessageStore.Dashboard.Models
{
    public class Message 
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}