local.settings.json
===================

To be able to successfully run this application locally with Visual Studio, you need to add a `local.settings.json` file to the same where this documentation file is located in.

The contents of this configuration file should be:

``` JSON
{
  "Api": {
    "BaseUri": string,
    "Scopes": string[]
  },
  "Application": {
    "ClientId": string,
    "ClientSecret": string,
    "TenantId": string
  }
}
```

The values are:
- `Api:BaseUri` - The base URI of your API. If you are running locally in Visual Studio, this would be `http://localhost:7071/api`
- `Api:Scopes` - An array of scopes you want to include in the token. Scopes represent the permissions that you have granted the calling application, so you could just specify a scope that represents all the scopes that have been granted. A scope always includes the App ID URI of the API you are calling. By default, this is `api://[Application ID]`, where `Application ID` is the Client ID of your API application. To specify the default scopes, you just append `/.default` to the scope. This would result in a scope `api://[Application ID]/.default`.
- `Application:ClientId` - The Client ID of your application that you are calling the API as.
- `Application:ClientSecret` - The client secret of the application you are calling the API as.
- `Application:TenantId` - The tenant ID of the application where the client applicaiton is registered. This can either be the guid of the tenant or the name, i.e. `[tenant name].onmicrosoft.com`.