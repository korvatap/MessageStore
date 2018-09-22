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
        #region GET

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

        #endregion

        #region POST

        [HttpPost(Name= "CreateMessage")]
        public IActionResult CreateMessage([FromBody] BaseMessage messageToCreate) 
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

        #endregion

        #region PUT

        #endregion

        #region PATCH

        #endregion

        #region DELETE

        [HttpDelete("{id}")]
        public IActionResult DeleteMessage(int messageId) 
        {
            var message = MessageDataStore.Current.Messages.FirstOrDefault(c => c.Id == messageId);

            if(message == null)
            {
                return NotFound();
            }

            MessageDataStore.Current.Messages.Remove(message);

            return NoContent();
        }

        #endregion
    }
}