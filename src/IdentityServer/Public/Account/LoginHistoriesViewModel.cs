namespace IdentityServer.Public.Account
{
    public class LoginHistoriesViewModel
    {
        public int Id { get; set; }
        public string TimeLogin { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string IpAddress { get; set; }
        public string Os { get; set; }
        public string Browser { get; set; }
    }
}
