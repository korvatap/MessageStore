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
        private readonly IMessageDataStore _current;

        public MessageController(IMessageDataStore messageDataStore)
        {
            _current = messageDataStore;
        }

        #region GET

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(_current.GetMessages());
        }

        [HttpGet("{id}")]
        public IActionResult GetMessage(int id)
        {
            Message messageToReturn = _current.GetMessages().FirstOrDefault(city => city.Id == id);
            
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

            int numberOfMessages = _current.GetNumberOfMessages();

            var messageToAdd = new Message
            {
                Id = ++numberOfMessages,
                Title = messageToCreate.Title,
                Body = messageToCreate.Body
            };

            _current.AddMessage(messageToAdd);

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
            var message = _current.GetMessages().FirstOrDefault(c => c.Id == messageId);

            if(message == null)
            {
                return NotFound();
            }

            _current.RemoveMessage(message);

            return NoContent();
        }

        #endregion
    }
}