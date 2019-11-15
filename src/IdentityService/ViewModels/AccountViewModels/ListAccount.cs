using System.Collections.Generic;

namespace IdentityService.ViewModels.AccountViewModels
{
    public class ListAccount
    {
        public string Count { get; set; }
        public List<Account> Data { get; set; }
    }
}
