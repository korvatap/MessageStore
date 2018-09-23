using MessageStore.API.Controllers;
using MessageStore.API.Models;
using MessageStore.API.Storage;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MessageStore.API.Tests
{
    public class MessageControllerShould
    {
        readonly MessageController _controller;
        private const int numberOfMessages = 100;

        public MessageControllerShould()
        {
            var storage = new MessageDataStore();
            //testdata
            for (int i = 0; i < numberOfMessages; i++)
            {
                var message = new Message
                {
                    Title = $"TestMessageTitle{i}",
                    Body = $"TestMessageBody{i}"
                };

                storage.AddMessage(message);
            }

            _controller = new MessageController(storage);

            // model validation does not work in unit testing, have to mock it
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o =>
                o.Validate(It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<Object>()));

            _controller.ObjectValidator = objectValidator.Object;
        }

        [Fact]
        public void ReturnOkWhenGetMessagesCalled()
        {
            IActionResult result = _controller.GetMessages();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ReturnAllTestMessagesFromGetMessages()
        {
            IActionResult result = _controller.GetMessages();
            var okResult = result as OkObjectResult;

            var storageMessages = Assert.IsType<List<Message>>(okResult.Value);
            Assert.Equal(numberOfMessages, storageMessages.Count);
        }

        [Fact]
        public void ReturnOkWhenGetMessageCalled()
        {
            IActionResult result = _controller.GetMessage(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ReturnMessageFromGetMessage()
        {
            IActionResult result = _controller.GetMessage(1);
            var okResult = result as OkObjectResult;

            var message = Assert.IsType<Message>(okResult.Value);
            Assert.NotNull(message);
        }

        [Fact]
        public void NotReturnMessageFromGetMessageWithWrongId()
        {
            IActionResult result = _controller.GetMessage(numberOfMessages+1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ReturnCreatedAtRouteWhenCreateMessageCalled()
        {
            var messageToCreate = new MessageDto
            {
                Title = "createMessageTitle",
                Body = "createMessageBody"
            };

            IActionResult result = _controller.CreateMessage(messageToCreate);
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public void ReturnMessageWhenCreateMessageCalled()
        {
            string title = "createMessageTitle";
            string body = "createMessageBody";
            var messageToCreate = new MessageDto
            {
                Title = title,
                Body = body
            };

            IActionResult result = _controller.CreateMessage(messageToCreate);
            var okResult = result as CreatedAtRouteResult;

            var message = Assert.IsType<Message>(okResult.Value);
            Assert.NotNull(message);
            Assert.Equal(title, message.Title);
            Assert.Equal(body, message.Body);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("createMessageTitle", "createMessageTitle")]
        public void ReturnBadRequestWhenCreateMessageCalledWithInvalidData(string title, string body)
        {
            var messageToCreate = new MessageDto
            {
                Title = title,
                Body = body
            };

            IActionResult result = _controller.CreateMessage(messageToCreate);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void FailModelValidationWhenTooLongTitle()
        {
            var model = new MessageDto
            {
                Title = new string('a', 101),
                Body = "test"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(result, x => x.ErrorMessage == "The field Title must be a string or array type with a maximum length of '100'.");
            Assert.Single(failure.MemberNames, x => x == "Title");
        }

        [Fact]
        public void FailModelValidationWhenTitleMissing()
        {
            var model = new MessageDto
            {
                Body = "test"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(result, x => x.ErrorMessage == "The Title field is required.");
            Assert.Single(failure.MemberNames, x => x == "Title");
        }

        [Fact]
        public void FailModelValidationWhenTooLongBody()
        {
            var model = new MessageDto
            {
                Title = "test",
                Body = new string('a', 501)
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(result, x => x.ErrorMessage == "The field Body must be a string or array type with a maximum length of '500'.");
            Assert.Single(failure.MemberNames, x => x == "Body");
        }

        [Fact]
        public void FailModelValidationWhenBodyMissing()
        {
            var model = new MessageDto
            {
                Title = "test"
            };

            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(result, x => x.ErrorMessage == "The Body field is required.");
            Assert.Single(failure.MemberNames, x => x == "Body");
        }

        [Fact]
        public void ReturnNoContentWhenUpdateMessageCalled()
        {
            var updatedMessage = new MessageDto
            {
                Title = "updatedMessageTitle",
                Body = "updatedMessageBody"
            };

            IActionResult result = _controller.UpdateMessage(1, updatedMessage);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateMessageOnUpdateMessageCalled()
        {
            string title = "updatedMessageTitle";
            string body = "updatedMessageBody";
            var updatedMessage = new MessageDto
            {
                Title = title,
                Body = body
            };

            IActionResult result = _controller.UpdateMessage(1, updatedMessage);
            Assert.IsType<NoContentResult>(result);

            IActionResult getResult = _controller.GetMessage(1);
            var okResult = Assert.IsType<OkObjectResult>(getResult);

            var message = Assert.IsType<Message>(okResult.Value);
            Assert.NotNull(message);
            Assert.Equal(title, message.Title);
            Assert.Equal(body, message.Body);
        }

        [Fact]
        public void ReturnNoContentWhenPartiallyUpdateMessageBodyCalled()
        {
            var updatedMessage = new MessageDto
            {
                Body = "updatedMessageBody"
            };

            var patchDoc = new JsonPatchDocument<MessageDto>();
            patchDoc.Replace(e => e.Body, updatedMessage.Body);

            IActionResult result = _controller.PartiallyUpdateMessage(1, patchDoc);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void ReturnNoContentWhenPartiallyUpdateMessageTitleCalled()
        {
            var updatedMessage = new MessageDto
            {
                Title = "updatedTestMessageTitle1"
            };

            var patchDoc = new JsonPatchDocument<MessageDto>();
            patchDoc.Replace(e => e.Title, updatedMessage.Title);

            IActionResult result = _controller.PartiallyUpdateMessage(1, patchDoc);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void ReturnNoContentWhenDeleteMessageCalled()
        {
            IActionResult result = _controller.DeleteMessage(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void ReturnNotFoundWhenDeleteMessageCalledOnNonExistingMessage()
        {
            IActionResult result = _controller.DeleteMessage(numberOfMessages+1);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
