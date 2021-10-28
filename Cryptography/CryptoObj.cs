namespace Profile_Database_Editor.Cryptography
{
    public class CryptoObj
    {
         public class CryptographicData
         {
                public string TextData { get; set; }
        
                public bool IsDataCorrect = true;
         }
         public enum EeryptAlgorithm
         {
             Des,
             Rsa
         }



    }
}