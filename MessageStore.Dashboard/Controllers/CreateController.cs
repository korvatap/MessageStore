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
    public class CreateController : Controller
    {
        private readonly HttpClient _client;

        public CreateController(HttpClient client) 
        {
            _client = client;
        }

        public IActionResult Index(string errorMessage = null, MessageDto message = null)
        {
            if (errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
                if (message != null)
                {
                    return View(message);
                }
            }

            return View();
        }

        [ActionName("save")]
        public async Task<IActionResult> PostAsync(MessageDto messageToSave)
        {
            string json = JsonConvert.SerializeObject(messageToSave, Formatting.None);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync($"api/messages/", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string message = "Please check your input data. Make sure both fields are not empty and Title cannot equal to Body";
                return RedirectToAction("Index", new {errorMessage = message, message = messageToSave});
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}