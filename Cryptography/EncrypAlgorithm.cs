using System;
using System.Collections.Generic;
using Profile_Database_Editor.Data;

namespace Profile_Database_Editor.Cryptography
{
    public class EncryptionAlgorithm
    {
        private  string _key;


        public virtual string Encrypt(string text)
        {
            throw new NotImplementedException();
        }
        
        
        public virtual CryptoObj.CryptographicData Decrypt(string text)
        {
            throw new NotImplementedException();
        }
        
        public List<UserData> EncryptionUserData(List<UserData> userData)
        {
            List<UserData> encryptedUserData = new List<UserData>();

            for (int i = 0; i < userData.Count; i++)
            {

                UserData eUserData = new UserData();


                eUserData.Id = userData[i].Id;
                eUserData.UserName = Encrypt(userData[i].UserName);
                eUserData.Email = Encrypt(userData[i].Email);
                eUserData.Password = Encrypt(userData[i].Password);
                    
                
                encryptedUserData.Add(eUserData);
                
            }
            

            return encryptedUserData;
        }

     

        public List<UserData> DecryptionUserData(List<UserData> encryptedUserData)
        {
            List<UserData> decryptedUserData = new List<UserData>();

            for (int i = 0; i < encryptedUserData.Count; i++)
            {

                UserData eUserData = new UserData();

                CryptoObj.CryptographicData cdUserName = Decrypt(encryptedUserData[i].UserName);
                CryptoObj.CryptographicData cdEmail = Decrypt(encryptedUserData[i].Email);
                CryptoObj.CryptographicData cdPassword = Decrypt(encryptedUserData[i].Password);
                
                eUserData.Id = encryptedUserData[i].Id;
                eUserData.UserName = cdUserName.TextData;
                eUserData.Email = cdEmail.TextData;
                eUserData.Password = cdPassword.TextData;

                if (cdUserName.IsDataCorrect == false || cdEmail.IsDataCorrect == false || cdPassword.IsDataCorrect == false)
                    eUserData.IsCorrectData = false;
                
                
                decryptedUserData.Add(eUserData);
                
            }

           
            
           
            return decryptedUserData;
        }

        public UserData EncryptionUserData(UserData userData)
        {
            UserData encryptedUserData = new UserData();

          
            encryptedUserData.Id = userData.Id;
            encryptedUserData.UserName = Encrypt(userData.UserName);
            encryptedUserData.Email = Encrypt(userData.Email);
            encryptedUserData.Password = Encrypt(userData.Password);
            
          

            return encryptedUserData;
        }
        
        public UserData DecryptionUserData(UserData encryptedUserData)
        {
            UserData decryptedUserData = new UserData();


            decryptedUserData.Id = encryptedUserData.Id;
            decryptedUserData.UserName = Decrypt(encryptedUserData.UserName).TextData;
            decryptedUserData.Email = Decrypt(encryptedUserData.Email).TextData;
            decryptedUserData.Password = Decrypt(encryptedUserData.Password).TextData;
                    
                
            return decryptedUserData;
        }
        
        
        
    }
}