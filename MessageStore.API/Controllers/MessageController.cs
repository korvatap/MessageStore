using System;
using System.Collections.Generic;
using System.Linq;
using MessageStore.API.Models;
using MessageStore.API.Storage;
using Microsoft.AspNetCore.Mvc;

namespace MessageStore.API.Controllers
{
    [Route("api/messages")]
    public class MessageController : Controller
    {
        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(MessageDataStore.Current.Messages);
        }

        [HttpGet("{id}")]
        public IActionResult GetMessage(int id)
        {
            Message messageToReturn = MessageDataStore.Current.Messages.FirstOrDefault(city => city.Id == id);
            
            if(messageToReturn == null) 
            {
                return NotFound();
            }

            return Ok(messageToReturn);
        }

        [HttpPost(Name= "CreateMessage")]
        public IActionResult CreatePMessage([FromBody] BaseMessage messageToCreate) 
        {
            if(messageToCreate == null)
                return BadRequest();

            if(messageToCreate.Title == messageToCreate.Body)
            {
                ModelState.AddModelError("Body", "The provided Body should be different from the title.");
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            int numberOfMessages = MessageDataStore.Current.NumberOfMessages;

            var messageToAdd = new Message
            {
                Id = ++numberOfMessages,
                Title = messageToCreate.Title,
                Body = messageToCreate.Body
            };

            MessageDataStore.Current.Messages.Add(messageToAdd);

            return CreatedAtRoute("GetPointOfInterest", new 
            {
                messageToAdd.Id
            }, messageToAdd);
        }
    }
}