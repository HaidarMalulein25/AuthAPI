using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthentication.Models
{
    public class RefreshTokenRequest
    {
        public string id { get; set; }
        public string accesstoken { get; set; }
        public string refreshtoken { get; set; }
    }
}
