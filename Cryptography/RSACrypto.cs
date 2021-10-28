using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Profile_Database_Editor.Data;

namespace Profile_Database_Editor.Cryptography
{
    class RsaCrypto : EncryptionAlgorithm
    {
        
        
        readonly string _pubKeyPath ;
        readonly string _priKeyPath ;
        public RsaCrypto(string pubKeyPath,string priKeyPath)
        {
            _pubKeyPath = pubKeyPath;
            _priKeyPath = priKeyPath;
        }

        public bool IsPrivateKeysExist()
        {
         
          return File.Exists(_priKeyPath);

        }
        public bool IsPubKeysExist()
        {
            return File.Exists(_pubKeyPath);
            
        }

        
        
        public void MakeKey()
        {
           
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(1024);
            
            RSAParameters privKey = csp.ExportParameters(true);
            
            RSAParameters pubKey = csp.ExportParameters(false);
            
            
            string pubKeyString;
            {
          
                var sw = new StringWriter();

                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
       
                xs.Serialize(sw, pubKey);
                
                pubKeyString = sw.ToString();
                File.WriteAllText(_pubKeyPath, pubKeyString);
            }
            string privKeyString;
            {
               
                var sw = new StringWriter();
               
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                
                xs.Serialize(sw, privKey);
              
                
                
                privKeyString = sw.ToString();
                File.WriteAllText(_priKeyPath, privKeyString);
            }
        }
        public override string Encrypt(string text)
        {
            
            string pubKeyString;
            {
                using (StreamReader reader = new StreamReader(_pubKeyPath))
                {
                    pubKeyString = reader.ReadToEnd();
                    
                }
            }
            
            var sr = new StringReader(pubKeyString);

            
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));

            
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            
            csp.ImportParameters((RSAParameters)xs.Deserialize(sr));
            
            byte[] bytesPlainTextData = Encoding.UTF8.GetBytes(text);

           
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);
            
            string encryptedText = Convert.ToBase64String(bytesCipherText);
            return encryptedText;

        }
        public override CryptoObj.CryptographicData Decrypt(string text)
        {
            CryptoObj.CryptographicData decryptData = new CryptoObj.CryptographicData();
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();


            try
            {
                string privKeyString = File.ReadAllText(_priKeyPath);
              
                var sr = new StringReader(privKeyString);
               
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
             
                RSAParameters privKey = (RSAParameters)xs.Deserialize(sr);
                csp.ImportParameters(privKey);
            
           
            
                byte[] bytesCipherText = Convert.FromBase64String(text);

           
                byte[] bytesPlainTextData = csp.Decrypt(bytesCipherText, false);

                decryptData.TextData = Encoding.UTF8.GetString(bytesPlainTextData);
                decryptData.IsDataCorrect = true;
                
                
                return  decryptData;
            }
            catch (Exception e)
            {
              
                CryptoObj.CryptographicData error = new CryptoObj.CryptographicData();
                error.TextData = "Error";
                error.IsDataCorrect = false;
                return error;
            }
            
            
        }

       
    }
}