using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MessageStore.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MessageStore.Dashboard.Controllers
{
    public class EditController : Controller
    {
        private readonly HttpClient _client;

        public EditController(HttpClient client) 
        {
            _client = client;
        }

        string Baseurl = "http://localhost:5000/";
        public async Task<ActionResult> Index(int? messageId)  
        {  
            if(messageId == null) 
            {
                return NotFound();
            }

            Message message = null;
            HttpResponseMessage Res = await _client.GetAsync($"api/messages/{messageId}");  
  
            if (Res.IsSuccessStatusCode)  
            {
                string response = Res.Content.ReadAsStringAsync().Result;  
                message = JsonConvert.DeserializeObject<Message>(response);  
            }

            return View(message);  
        }
    }
}