using System;
using MessageStore.API.Models;
using MessageStore.API.Storage;
using Microsoft.AspNetCore.JsonPatch;
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
        public IActionResult GetMessages()
        {
            return Ok(_current.GetMessages());
        }

        [HttpGet("{messageId}")]
        public IActionResult GetMessage(int messageId)
        {
            Message messageToReturn = _current.GetMessage(messageId);

            if (messageToReturn == null) 
                return NotFound();

            return Ok(messageToReturn);
        }

        #endregion

        #region POST

        [HttpPost(Name= "CreateMessage")]
        public IActionResult CreateMessage([FromBody] MessageDto messageToCreate) 
        {
            if(messageToCreate == null)
                return BadRequest();

            if(messageToCreate.Title == messageToCreate.Body)
            {
                ModelState.AddModelError("Body", "The provided Body should be different from the title.");
            }

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var messageToAdd = new Message
            {
                Title = messageToCreate.Title,
                Body = messageToCreate.Body
            };

            _current.AddMessage(messageToAdd);

            return CreatedAtRoute("CreateMessage", new
            {
                messageToAdd.Id
            }, messageToAdd);
        }

        #endregion

        #region PUT

        [HttpPut("{messageId}")]
        public IActionResult UpdateMessage(int messageId, [FromBody] MessageDto message)
        {
            if(message == null)
                return BadRequest();

            if(message.Body == message.Title)
            {
                ModelState.AddModelError("Body", "The provided Body should be different from the title.");
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Message messageFromStorage = _current.GetMessage(messageId);

            if(messageFromStorage == null)
                return NotFound();

            messageFromStorage.Title = message.Title;
            messageFromStorage.Body = message.Body;
            messageFromStorage.ModifiedAt = DateTime.Now;

            return NoContent();
        }

        #endregion

        #region PATCH

        [HttpPatch("{messageId}")]
        public IActionResult PartiallyUpdateMessage(int messageId, [FromBody] JsonPatchDocument<MessageDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            Message messageFromStorage = _current.GetMessage(messageId);

            if (messageFromStorage == null)
                return NotFound();

            var messageToPatch = new MessageDto
            {
                Title = messageFromStorage.Title,
                Body = messageFromStorage.Body
            };

            patchDoc.ApplyTo(messageToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (messageToPatch.Body == messageToPatch.Title)
            {
                ModelState.AddModelError("Body", "The provided body should be different from the title.");
            }

            TryValidateModel(messageToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            messageFromStorage.Title = messageToPatch.Title;
            messageFromStorage.Body = messageToPatch.Body;
            messageFromStorage.ModifiedAt = DateTime.Now;

            return NoContent();
        }

        #endregion

        #region DELETE

        [HttpDelete("{messageId}")]
        public IActionResult DeleteMessage(int messageId)
        {
            var message = _current.GetMessage(messageId);

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