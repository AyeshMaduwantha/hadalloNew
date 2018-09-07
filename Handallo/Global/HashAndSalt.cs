using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Handallo.Global
{
    public static class HashAndSalt
    {
        public static String  HashSalt(String password)
        {
            byte[] salt = new byte[128 / 8];
 
            //String newsalt = Convert.ToBase64String(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

    }
}
