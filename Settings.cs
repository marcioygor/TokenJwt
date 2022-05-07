using System;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Api_Carro.Models;
using System.Collections.Generic;
using System.Linq;

namespace Api_Carro
{
    public static class Settings
    {
        public static string Secret = "23bfc801cb81a2a8fb7d0e4d42ef5559";
    } 
}
