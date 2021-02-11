using System;
namespace UserAuthentication.Classes
{
    public class User
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public object salt { get; set; }
        public string token { get; set; }
        public string refresh_token { get; set; }
        public DateTime token_expiration { get; set; }
        public DateTime refresh_token_expiration { get; set; }
    }
}
