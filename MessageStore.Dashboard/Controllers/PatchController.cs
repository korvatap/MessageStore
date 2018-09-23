using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MessageStore.Dashboard.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MessageStore.Dashboard.Controllers
{
    public class PatchController : Controller
    {
        private readonly HttpClient _client;

        public PatchController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index(int? messageId, bool patchTitle)
        {
            if (messageId == null)
            {
                return NotFound();
            }

            Message message = null;
            HttpResponseMessage result = await _client.GetAsync($"api/messages/{messageId}");

            if (result.IsSuccessStatusCode)
            {
                string response = await result.Content.ReadAsStringAsync();
                message = JsonConvert.DeserializeObject<Message>(response);
            }

            ViewBag.PatchTitle = patchTitle;

            return View(message);
        }

        [ActionName("patchtitle")]
        public async Task<IActionResult> PatchTitleAsync(Message message)
        {
            if (message.Title == null)
                return BadRequest();

            var patchDoc = new JsonPatchDocument<MessageDto>();
            patchDoc.Replace(e => e.Title, message.Title);

            string serializedPatch = JsonConvert.SerializeObject(patchDoc, Formatting.None);
            var httpContent = new StringContent(serializedPatch, Encoding.UTF8, "application/json");

            string url = $"api/messages/{message.Id}";
            HttpResponseMessage response = await _client.PatchAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ActionName("patchbody")]
        public async Task<IActionResult> PatchBodyAsync(Message message)
        {
            if (message.Body == null)
                return BadRequest();

            var patchDoc = new JsonPatchDocument<MessageDto>();
            patchDoc.Replace(e => e.Body, message.Body);

            var serializedPatch = JsonConvert.SerializeObject(patchDoc);
            var httpContent = new StringContent(serializedPatch, Encoding.UTF8, "application/json");

            string url = $"api/messages/{message.Id}";
            HttpResponseMessage response = await _client.PatchAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new ErrorViewModel{ RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
