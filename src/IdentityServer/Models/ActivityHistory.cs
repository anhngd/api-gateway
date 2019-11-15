using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class ActivityHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public string Email { get; set; }
        public string TimeLogin { get; set; }
        public string UserId { get; set; }
        public string Location { get; set; }
        public string IpAddress { get; set; }
        public string Os { get; set; }
        public string Browser { get; set; }
        public string Action { get; set; }
    }
}