using System.ComponentModel.DataAnnotations;

namespace UserAuthentication.Models
{
    public class CRUDRequest
    {
        public string id { get; set; }
        [Required]
        public string accesstoken { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}
