local.settings.json
===================

To be able to run this application locally, you need to make sure you have a `local.settings.json` file in the same folder with this documentation.

The contents of this file should be:

``` JSON
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",

    "Application:ClientId": string,
    "Application:ClientSecret": string,
    "Application:TenantId": string
  }
}
```

The values in the configuration are:

- `Application:ClientId` - The client ID of your API application.
- `Application:ClientSecret` - The client secret that goes with the client ID.
- `Application:TenantId` - The tenant ID of the tenant in which the API application is registered. This can be the guid or name that represents the tenant (`[tenant name].onmicrosoft.com`)

Please note that these are actually not required, since the API application does not need to identify itself to another API for instance. But when you start creating real-world applications, you most certainly need to identify your API application as well, for instance if you need to call into Microsoft Graph.