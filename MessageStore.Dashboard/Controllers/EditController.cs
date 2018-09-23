using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
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

        public async Task<ActionResult> Index(int? messageId, string errorMessage = null, Message messageToFix = null)  
        {  
            if(messageId == null) 
            {
                return NotFound();
            }

            if (errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
                if (messageToFix != null)
                {
                    return View(messageToFix);
                }
            }

            Message message = null;
            HttpResponseMessage response = await _client.GetAsync($"api/messages/{messageId}");  
  
            if (response.IsSuccessStatusCode)  
            {
                string result = await response.Content.ReadAsStringAsync();  
                message = JsonConvert.DeserializeObject<Message>(result);  
            }

            return View(message);  
        }

        [ActionName("save")]
        public async Task<IActionResult> PutAsync(Message messageToSave)
        {
            string json = JsonConvert.SerializeObject(messageToSave, Formatting.None);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PutAsync($"api/messages/{messageToSave.Id}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string message = "Please check your input data. Make sure both fields are not empty and Title cannot equal to Body";
                return RedirectToAction("Index", new { errorMessage = message, messageToFix = messageToSave });
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}