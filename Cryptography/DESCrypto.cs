using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Profile_Database_Editor.Data;

namespace Profile_Database_Editor.Cryptography
{
   
    
    public class DesCrypto : EncryptionAlgorithm
    {
        private  string _key;

        public DesCrypto(string key)
        {
            _key = key;
        }



        public override string Encrypt(string text)
        {
            try
            {
            
                byte[] hashKey = new MD5CryptoServiceProvider().ComputeHash(UTF8Encoding.UTF8.GetBytes(_key));
                
                
                TripleDESCryptoServiceProvider tds = new TripleDESCryptoServiceProvider();
                tds.Key = hashKey;
                tds.Mode = CipherMode.ECB;
                tds.Padding = PaddingMode.PKCS7;

                ICryptoTransform transform = tds.CreateEncryptor();
                
                byte[] bytesText = Encoding.UTF8.GetBytes(text);
                byte[] result = transform.TransformFinalBlock(bytesText, 0, bytesText.Length);
              
                return Convert.ToBase64String(result);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        
        public override CryptoObj.CryptographicData Decrypt(string text)
        {
            CryptoObj.CryptographicData decryptCryptographicData = new CryptoObj.CryptographicData();
            try
            {
                
                byte[] bytesText = Convert.FromBase64String(text.Replace("\0", null));
                
                byte[] hashKey = new MD5CryptoServiceProvider().ComputeHash(UTF8Encoding.UTF8.GetBytes(_key));
                
                TripleDESCryptoServiceProvider tds = new TripleDESCryptoServiceProvider();
                tds.Key = hashKey;
                tds.Mode = CipherMode.ECB;
                tds.Padding = PaddingMode.PKCS7;

                ICryptoTransform transform = tds.CreateDecryptor();
                
                
                byte[] result = transform.TransformFinalBlock(bytesText, 0, bytesText.Length);

                decryptCryptographicData.TextData = Encoding.UTF8.GetString(result);
                decryptCryptographicData.IsDataCorrect = true;
                
                return decryptCryptographicData;
                
            }
            catch (Exception ex)
            {
                string error = default;
                //Normal error interpretation
                if (ex.Message == "Padding is invalid and cannot be removed.")
                    error = "Wrong key";
                else if (ex.Message == "The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.")
                    error = "Data is not encrypted";
                
                decryptCryptographicData.TextData = error;
                decryptCryptographicData.IsDataCorrect = false;
                
                return decryptCryptographicData;
            }
        }
    }
}