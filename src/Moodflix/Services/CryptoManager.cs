using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CryptoManager
    {
        public static string Hash(string input)
        {

            using (SHA256 hash = SHA256.Create())
            {
                var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sb = new StringBuilder();

                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }

        }

        //Clave hash admin = 8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918

        public static bool Compare(string passwordIngresada, string passwordGuardada)
        {
            return Hash(passwordIngresada).Equals(passwordGuardada);
        }

    }
}
