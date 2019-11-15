using System.Collections.Generic;

namespace IdentityService.ViewModels.UserViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ListUser
    {
        public int Count { get; set; }
        public List<User> Data { get; set; }
    }
}
