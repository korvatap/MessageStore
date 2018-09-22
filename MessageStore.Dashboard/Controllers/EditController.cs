using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MessageStore.Dashboard.Controllers
{
    public class Message 
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }

    public class EditController : Controller
    {
        string Baseurl = "http://localhost:5000/";
        public async Task<ActionResult> Index(int? messageId)  
        {  
            if(messageId == null) 
            {
                return NotFound();
            }

            Message message = null;
              
            using (var client = new HttpClient())  
            {  
                //Passing service base url  
                client.BaseAddress = new System.Uri(Baseurl);  
  
                client.DefaultRequestHeaders.Clear();  
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
                  
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync($"api/messages/{messageId}");  
  
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)  
                {  
                    //Storing the response details recieved from web api   
                    var response = Res.Content.ReadAsStringAsync().Result;  
  
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    message = JsonConvert.DeserializeObject<Message>(response);  
  
                }  
                //returning the employee list to view  
                return View(message);  
            }  
        }  
    }
}