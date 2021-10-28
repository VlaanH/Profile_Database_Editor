using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Profile_Database_Editor.Cryptography;
using Profile_Database_Editor.Message;
using Profile_Database_Editor.Settings;

namespace Profile_Database_Editor
{
    public class ASINKkey : Window
    {
        public ASINKkey()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            LoadingSettings();
        }

        public async void LoadingSettings()
        {
            var settings= await SettingsManagement.GetKey();
            
            this.Find<TextBox>("privBox").Text=settings.PriKeyPath;
            this.Find<TextBox>("pubBox").Text=settings.PubKeyPath;
        }

        public static void Show(Window parent)
        {
            var msgbox = new ASINKkey();
            msgbox.ShowDialog(parent);
        }

        public SettingsDataKeys GetEntryKeys()
        {

            SettingsDataKeys keys = new SettingsDataKeys();
            keys.PriKeyPath = this.Find<TextBox>("privBox").Text;
            keys.PubKeyPath = this.Find<TextBox>("pubBox").Text;
            
            return keys;
        }

        
        
        private void MakeButton_OnClick(object? sender, RoutedEventArgs e)
        {

            SettingsDataKeys keys = new SettingsDataKeys();
            keys.PubKeyPath = GetEntryKeys().PubKeyPath;
            keys.PriKeyPath = GetEntryKeys().PriKeyPath;
            
            
            if (keys.PriKeyPath == keys.PubKeyPath)
            {
                keys.PriKeyPath += "pr.key";
                keys.PubKeyPath += "pu.key";
            }    
                
            
            
            try
            {
                RsaCrypto rsaCrypto = new RsaCrypto(keys.PubKeyPath, keys.PriKeyPath);
                rsaCrypto.MakeKey();
                
                this.Find<TextBox>("privBox").Text=keys.PriKeyPath;
                this.Find<TextBox>("pubBox").Text=keys.PubKeyPath;
                
                
                SaveKeys(keys);

                
                
            }
            catch (Exception exception)
            {
               MessageDialog.ShowMessage("key creation error");
            }
            
            
        }

        void SaveKeys(SettingsDataKeys keys )
        {
           
           
            SettingsManagement.SaveKey(keys);    
        }

        private void SaveButton_OnClick(object? sender, RoutedEventArgs e)
        {
            
            SaveKeys(GetEntryKeys());
        }

        private async void PrivButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<TextBox>("privBox").Text= await SettingsManagement.PatchDialog.GetFilePatch(this);
        }

        private async void PubButton_OnClick(object? sender, RoutedEventArgs e)
        {  
            this.Find<TextBox>("pubBox").Text= await SettingsManagement.PatchDialog.GetFilePatch(this);
           
        }
    }
}