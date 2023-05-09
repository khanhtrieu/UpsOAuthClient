# UpsOAuthClient
Use to Create authorization tokens for your application to utilize UPS APIs. This API product is required for all UPS's API integrations.
## Installation
```
Install-Package UpsOAuthClient -Version 1.0.3
```
See NuGet docs for additonal instructions on installing via the dialog or the console.

## Usage
A simple create token example:

```
using OAuthClient

OAuthClient.OAuthClient oAuth = new OAuthClient(
    new OAuthClient.Http.ClientConfiguration(clientKey, clientSecret, 
	OAuthClient.Common.Enums.ApiEnvironment.Test) );
	
var token = Task.Run(async()=>await client.CreateToken());
```

## Testing
The test suite in this project was specifically build to produce 

### Settings
1. Rename  ```appsettings - sample.json``` to ```appsettings.json```
2. Fill up the following configuration
	* __ClientID__: Clinet ID provided by UPS when create your application.
	* __ClientSecret__: Clinet Secret provided by UPS when create your application
3. Change ```appsettings.json``` property __Copy to Output Directory__ to **``Copy always``** or **``Copy if newer``**
	