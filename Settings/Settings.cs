using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Profile_Database_Editor.Settings
{
    public class SettingsData 
    {
        public string DatabasePath { get; set; }
            
        public string EncryptionKey { get; set; }
            
        public bool UseEncryption { get; set; }
        
        public Database.EModels Table { get; set; }
        
      
    }

    public class SettingsDataKeys
    {
        public string  PubKeyPath{ get; set; }
        
        public string  PriKeyPath{ get; set; }
        
    }

   
    
    public class SettingsManagement
    {
        public static class PatchDialog
        {
            public static async Task<string?> GetFilePatch(Window mainWindow)
            {
                var dialog = new OpenFileDialog();
            
                var result = await dialog.ShowAsync(mainWindow);

                if (result != null)
                    return result[0];
                else
                    return default;

            }
            public static async Task<string?> GetFolderPatch(Window mainWindow)
            {
                var dialog = new OpenFolderDialog();
            
                var result = await dialog.ShowAsync(mainWindow);

                if (result != null)
                    return result;
                else
                    return default;

            }


   
        }
        public static async Task<SettingsData> Get()
        {
            SettingsData settings = new SettingsData();
            
           
            try
            {
                using (FileStream fs = new FileStream("Settings.json", FileMode.OpenOrCreate))
                {
                    settings = await JsonSerializer.DeserializeAsync<SettingsData>(fs);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            
                

            return settings;
          
        }
        public static async void Save(SettingsData settingsClass)
        {
            
            using (FileStream fs = new FileStream("Settings.json", FileMode.OpenOrCreate))
            { 
                //cleaning
                fs.SetLength(default);
                
                await JsonSerializer.SerializeAsync<SettingsData>(fs, settingsClass);
            }
        }
        
        public static async Task<SettingsDataKeys> GetKey()
        {
            SettingsDataKeys settings = new SettingsDataKeys();
            
           
            try
            {
                using (FileStream fs = new FileStream("SettingsKeys.json", FileMode.OpenOrCreate))
                {
                    settings = await JsonSerializer.DeserializeAsync<SettingsDataKeys>(fs);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            
                

            return settings;
          
        }
        public static async void SaveKey(SettingsDataKeys settingsClass)
        {
            
            using (FileStream fs = new FileStream("SettingsKeys.json", FileMode.OpenOrCreate))
            { 
                //cleaning
                fs.SetLength(default);
                
                await JsonSerializer.SerializeAsync<SettingsDataKeys>(fs, settingsClass);
            }
        }
        
    
        
        
        
    }

}