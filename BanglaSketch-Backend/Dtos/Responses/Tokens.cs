using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Responses
{
    public class Tokens
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
