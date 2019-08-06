using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Windows_UWP.Utils
{
    public static class JWTTokenConverter
    {
        public static List<Claim> ConvertToList(string jwtEncodedString)
        {
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            return token.Claims.ToList();
        }
    }
}
