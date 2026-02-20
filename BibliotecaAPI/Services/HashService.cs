using BibliotecaAPI.DTOs.Auth;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BibliotecaAPI.Services
{
    public class HashService : IHashService
    {
        public HashResponseDTO Hash(string input)
        {
            var sal = new Byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);
            }

            return Hash(input, sal);
        }

        public HashResponseDTO Hash(string input, byte[] sal)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: sal,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10_000,
                numBytesRequested: 256 / 8
                ));

            return new HashResponseDTO
            {
                Hash = hashed,
                Sal = sal
            };
        }
    }
}
