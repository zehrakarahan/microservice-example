using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeCenter.Dto
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        //
        // 摘要:
        //     Gets the identity token.
        public string IdentityToken { get; set; }
        //
        // 摘要:
        //     Gets the type of the token.
        public string TokenType { get; set; }
        //
        // 摘要:
        //     Gets the refresh token.
        public string RefreshToken { get; set; }
        //
        // 摘要:
        //     Gets the error description.
        public string ErrorDescription { get; set; }
        //
        // 摘要:
        //     Gets the expires in.
        public int ExpiresIn { get; set; }
    }
}
