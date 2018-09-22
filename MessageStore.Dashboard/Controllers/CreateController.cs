using System.Net.Http;
using System.Threading.Tasks;
using MessageStore.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MessageStore.Dashboard.Controllers
{
    public class CreateController : Controller
    {
        private readonly HttpClient _client;

        public CreateController(HttpClient client) 
        {
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveAsync([FromBody] MessageDto messageToSave)
        {
            string json = JsonConvert.SerializeObject(messageToSave, Formatting.None    );
            var httpContent = new StringContent(json);

            Message message;
            HttpResponseMessage response = await _client.PostAsync($"api/messages/", httpContent);  
  
            if (response.IsSuccessStatusCode)  
            {
                string result = response.Content.ReadAsStringAsync().Result;  
                message = JsonConvert.DeserializeObject<Message>(result);  
            }

            return Index();
        }
    }
}