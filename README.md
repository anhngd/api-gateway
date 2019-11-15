# api-gateway

Sample of configuration block:

```json
{
  "DownstreamPathTemplate": "/{everything}",
  "UpstreamPathTemplate": "/{everything}",
  "UpstreamHttpMethod": [
    "Get",
    "Post",
    "Put",
    "Patch",
    "Delete",
    "Options"
  ],
  "AddHeadersToRequest": {},
  "AddClaimsToRequest": {},
  "RouteClaimsRequirement": {},
  "AddQueriesToRequest": {},
  "RequestIdKey": "",
  "FileCacheOptions": {
    "TtlSeconds": 0,
    "Region": ""
  },
  "ReRouteIsCaseSensitive": true,
  "ServiceName": "",
  "DownstreamScheme": "https",
  "DownstreamHostAndPorts": [
    {
      "Host": "jsonplaceholder.typicode.com",
      "Port": 443
    }
  ],
  "QoSOptions": {
    "ExceptionsAllowedBeforeBreaking": 0,
    "DurationOfBreak": 0,
    "TimeoutValue": 0
  },
  "LoadBalancer": "",
  "RateLimitOptions": {
    "ClientWhitelist": [],
    "EnableRateLimiting": false,
    "Period": "",
    "PeriodTimespan": 0,
    "Limit": 0
  },
  "AuthenticationOptions": {
    "AuthenticationProviderKey": "",
    "AllowedScopes": []
  },
  "HttpHandlerOptions": {
    "AllowAutoRedirect": true,
    "UseCookieContainer": true,
    "UseTracing": true
  },
  "DangerousAcceptAnyServerCertificateValidator": true
}
```
