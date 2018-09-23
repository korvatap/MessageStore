# MessageStore
## This repository contains three projects
- MessageStore.API
- MessageStore.Dashboard
- MessageStore.API.Tests

## Requirements
.NET Core SDK 2.1. You can download it from https://www.microsoft.com/net/download.

## How to run
### API & Dashboard
- Open command prompt/terminal etc.
- Navigate to desired project folder
- Run command "dotnet run"
### Tests
- Open command prompt/terminal etc.
- Navigate to desired project folder
- Run command "dotnet test"

## MessageStore.API
This project is a RESTful API which works as a message store.
This API supports GET, POST, PUT, PATCH, DELETE HTTP request method.
Accepted Content-Type: application/json
### Get requests
  #### Get all messages in store
  javascript example:
  ```javascript
  var settings = {
    "async": true,
    "crossDomain": true,
    "url": "http://localhost:5000/api/messages",
    "method": "GET",
    "headers": {
      "content-type": "application/json"
    }
  }

$.ajax(settings).done(function (response) {
   console.log(response);
});
```
#### Get specific message
javascript example:
```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "http://localhost:5000/api/messages/1",
  "method": "GET",
  "headers": {
    "content-type": "application/json",
    "cache-control": "no-cache"
  }
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```
### POST requests
#### Create new message to store
javascript example:
```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "http://localhost:5000/api/messages/",
  "method": "POST",
  "headers": {
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\"Title\": \"TestMessageTitle\",\"Body\": \"TestMessageBody\"}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```
### PUT requests
#### Update existing message with new values
javascript example:
```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "http://localhost:5000/api/messages/1",
  "method": "PUT",
  "headers": {
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "{\"Title\": \"UpdatedMessageTitle\",\"Body\": \"UpdatedMessageBody\"}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
````
### PATCH
#### Update title of existing message
javascript example:
```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "http://localhost:5000/api/messages/1",
  "method": "PATCH",
  "headers": {
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "[{\"op\": \"replace\",\"path\": \"/Title\",\"value\": \"PatchUpdatedTitle\"}\r\n]"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```
#### Update body of existing message
javascript example:
```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "http://localhost:5000/api/messages/1",
  "method": "PATCH",
  "headers": {
    "content-type": "application/json",
    "cache-control": "no-cache"
  },
  "processData": false,
  "data": "[{\"op\": \"replace\",\"path\": \"/Body\",\"value\": \"PatchUpdatedBody\"}\r\n]"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```
### DELETE
#### Delete existing message
javascript example:
```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "http://localhost:5000/api/messages/1",
  "method": "DELETE",
  "headers": {
    "content-type": "application/json",
    "cache-control": "no-cache"
  }
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```
## MessageStore.Dashboard
This project is a Web application using ASP.NET Core MVC framework.
With this web application you can see the API in action.
In the UI you can execute all HTTP request methods mentioned above.

## MessageStore.API.Tests
This xunit test project to test the MessageStore.API.
This test project covers tests for MessageStorage and MessageStore controller behavior.
