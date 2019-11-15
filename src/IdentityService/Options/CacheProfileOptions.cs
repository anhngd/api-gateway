using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Options
{
    /// <summary>
    /// The caching options for the application.
    /// </summary>
    public class CacheProfileOptions : Dictionary<string, CacheProfile>
    {
    }
}
