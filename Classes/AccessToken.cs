using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthentication.Classes
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
