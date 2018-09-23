# MessageStore
## This repository contains three projects
- MessageStore.API
- MessageStore.Dashboard
- MessageStore.API.Tests

## Requirements
.NET Core SDK 2.1. You can download it from https://www.microsoft.com/net/download.

## How to get using git
- Open command prompt/terminal
- Navigate to desired folder where to clone the repository
- Run command `git clone git@github.com:korvatap/MessageStore.git`

## How to run
### API & Dashboard
- Open command prompt/terminal etc.
- Navigate to desired project folder
- Run command `dotnet run`
- (If you want to use Dashboard you need to also run the API)
### Tests
- Open command prompt/terminal etc.
- Navigate to desired project folder
- Run command `dotnet test`

## MessageStore.API
This project is a RESTful API which works as a message store.
This API supports GET, POST, PUT, PATCH, DELETE HTTP request method.
Accepted Content-Type: application/json

### Configuration MessageStore/MessageStore.API/Properties/launchSettings.json
To change url/port where API will listen, change applicationUrl field in json file.
example:

```
"MessageStore.API": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      },
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
  }
```

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

### Configuration - appsettings.json
Below is an config file where you can set MessageStoreApiUrl if you have changed API settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApplicationConfiguration": {
    "MessageStoreApiUrl": "http://localhost:5000/"
  }
}
```

## MessageStore.API.Tests
This xunit test project to test the MessageStore.API.
This test project covers tests for MessageStorage and MessageStore controller behavior.
