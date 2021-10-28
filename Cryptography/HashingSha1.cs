using System;
using System.Security.Cryptography;
using System.Text;

namespace Profile_Database_Editor.Cryptography
{
    public class HashingSha1
    {
       public static string Hash(string input)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
                
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                
                string hash = BitConverter.ToString(hashBytes).Replace("-",String.Empty).ToLower();

                
                return hash;
            }
            
        }
    }
}