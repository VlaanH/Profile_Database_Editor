using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Profile_Database_Editor.Cryptography;
using Profile_Database_Editor.Data;
using Profile_Database_Editor.Database;
using Profile_Database_Editor.InterfaceObjects;
using Profile_Database_Editor.Message;
using Profile_Database_Editor.Settings;
using Profile_Database_Editor.Cryptography;

namespace Profile_Database_Editor
{
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Initialization();

        }

        async void Initialization()
        {
            //Create Main StackPanel
            InterfaceDatabaseRecord.MainStackPanel.Background= SolidColorBrush.Parse("#292828");
            
            this.FindControl<ScrollViewer>("DbRecord").Content = InterfaceDatabaseRecord.MainStackPanel;
            
           var settings = await SettingsManagement.Get();
            SetSettings(settings);
            if (File.Exists(settings.DatabasePath))
            {
                AddTables(settings.DatabasePath,true);
            }
           
        }
        
        
        
        
        UserData GetEnteredData()
        {
            UserData enteredUserData = new UserData();
            enteredUserData.UserName = this.FindControl<TextBox>("UserNameTextBox").Text+"";
            enteredUserData.Password = this.FindControl<TextBox>("PasswordTextBox").Text+"";
            enteredUserData.Email = this.FindControl<TextBox>("EmailTextBox").Text+"";

            return enteredUserData;
        }

        SettingsData GetSettings()
        {
            SettingsData settings = new SettingsData();
            settings.UseEncryption = this.Find<ToggleSwitch>("EncryptionSwitch").IsChecked!.Value;
            settings.EncryptionKey = this.Find<TextBox>("TextBoxKey").Text;
            settings.DatabasePath = this.Find<TextBox>("TextBoxDbPath").Text;
            try
            {
                settings.Table = this.Find<ComboBox>("ComboBoxDatabaseTables").SelectedItem!.ToString();
            }
            catch (Exception e)
            {
                
            }
            
            return settings;
        }

        void SetSettings(SettingsData settingsData)
        {
            
            this.Find<ToggleSwitch>("EncryptionSwitch").IsChecked=settingsData.UseEncryption = settingsData.UseEncryption;
            this.Find<TextBox>("TextBoxKey").Text=settingsData.EncryptionKey = settingsData.EncryptionKey;
            this.Find<ComboBox>("ComboBoxDatabaseTables").SelectedIndex = 0;
            this.Find<TextBox>("TextBoxDbPath").Text = settingsData.DatabasePath;

        }

        string GetDatabasePath()
        {

            return this.Find<TextBox>("TextBoxDbPath").Text;
        }

        CryptoObj.EeryptAlgorithm GetSelectedEncryptionAlgorithm()
        {
            return (CryptoObj.EeryptAlgorithm) this.Find<ComboBox>("ComboBoxenEryptAlgorithm").SelectedIndex;
        }


        void SaveSettings()
        {
           var settings = GetSettings();
           SettingsManagement.Save(settings);
        }


        bool IsComboBoxEmpty()
        {
            bool isEmpty = false;
            try
            {
                this.Find<ComboBox>("ComboBoxDatabaseTables").SelectedItem!.ToString();
            }
            catch (Exception e)
            {
                isEmpty = true;

            }

            return isEmpty;
        }

        bool AddTables(string dpPath,bool init=false)
        {
            bool error = false;
            try
            {
                DatabaseManagement databaseManagement = new DatabaseManagement(dpPath);
            
                var allTables = databaseManagement.GetAllTables();
                if (allTables.Count < 1)
                    error = true;
             
              
            
            
                this.Find<ComboBox>("ComboBoxDatabaseTables").Items = allTables;
                databaseManagement.DbConnector.Close();
                if (init==true|IsComboBoxEmpty()==true)
                    this.Find<ComboBox>("ComboBoxDatabaseTables").SelectedIndex = 0;

            }
            catch (Exception e)
            {
                error = true;
            }
            
            

            return error;
        }
      
        
        
        

        async void Update()
        {
            var encryptionAlgorithm = GetSelectedEncryptionAlgorithm();
            var settingsKey = await SettingsManagement.GetKey();
            
            SettingsData settings = GetSettings();
            DatabaseManagement databaseManagement=default!;
            try
            {
                databaseManagement = new DatabaseManagement(settings.DatabasePath);
            }
            catch (Exception e)
            {
                
            }

            List<UserData> listAllRecordsDb = new List<UserData>();

            await Task.Run(() =>
            {
                try
                {
                    listAllRecordsDb = databaseManagement.GetAllRecords(settings.Table);

                    if (settings.UseEncryption == true)
                    {
                        if (encryptionAlgorithm== CryptoObj.EeryptAlgorithm.Des)
                        {
                            listAllRecordsDb = new DesCrypto(settings.EncryptionKey).DecryptionUserData(listAllRecordsDb);
                        }
                        else if (encryptionAlgorithm== CryptoObj.EeryptAlgorithm.Rsa)
                        {
                            listAllRecordsDb = new RsaCrypto(settingsKey.PubKeyPath,settingsKey.PriKeyPath ).DecryptionUserData(listAllRecordsDb);
                        }

                        
                        
                        
                    }

                    
                }
                catch (Exception e)
                {
                    MessageDialog.ShowMessage("Error in decrypting data");
                }


               
            });
            if (encryptionAlgorithm == CryptoObj.EeryptAlgorithm.Rsa)
            {
                settings.EncryptionKey = settingsKey.PubKeyPath;
            }
            
            InterfaceDatabaseRecord.DatabaseOutput(listAllRecordsDb,databaseManagement,settings.UseEncryption,settings.Table,encryptionAlgorithm,settings.EncryptionKey);

            

        }


        async Task<bool> Add()
        {
            bool error = false;
            var encryptionAlgorithm = GetSelectedEncryptionAlgorithm();
            var settingsKey = await SettingsManagement.GetKey();
            SettingsData settings = GetSettings();

            
            DatabaseManagement databaseManagement = new DatabaseManagement(settings.DatabasePath);
            
            
            List<UserData> listAllRecordsDb = new List<UserData>();
            UserData enteredUserData = GetEnteredData();
     
            
            await Task.Run(() =>
            {

                try
                {
                    if (settings.UseEncryption == true)
                    {
                        if (encryptionAlgorithm== CryptoObj.EeryptAlgorithm.Des)
                        {
                            enteredUserData= new DesCrypto(settings.EncryptionKey).EncryptionUserData(enteredUserData);
                        }
                        else if (encryptionAlgorithm== CryptoObj.EeryptAlgorithm.Rsa)
                        {
                            enteredUserData = new RsaCrypto(settingsKey.PubKeyPath,settingsKey.PriKeyPath ).EncryptionUserData(enteredUserData);
                        }
                    }
                
                    
                    databaseManagement.AddRecordDb(enteredUserData,settings.Table);
                   
             
                
                    listAllRecordsDb = databaseManagement.GetAllRecords(settings.Table);

                    if (settings.UseEncryption == true)
                    {
                        if (encryptionAlgorithm== CryptoObj.EeryptAlgorithm.Des)
                        {
                            listAllRecordsDb = new DesCrypto(settings.EncryptionKey).DecryptionUserData(listAllRecordsDb);
                        }
                        else if (encryptionAlgorithm== CryptoObj.EeryptAlgorithm.Rsa)
                        {
                            listAllRecordsDb = new RsaCrypto(settingsKey.PubKeyPath,settingsKey.PriKeyPath ).DecryptionUserData(listAllRecordsDb);
                        }
                    }
                }
                catch (Exception)
                {
                    error = true;
                }
                
                

                   
                return error;
                
                
                
            });
            if (encryptionAlgorithm == CryptoObj.EeryptAlgorithm.Rsa)
            {
                settings.EncryptionKey = settingsKey.PubKeyPath;
            }
            
            if (error==false)
                InterfaceDatabaseRecord.DatabaseOutput(listAllRecordsDb,databaseManagement,settings.UseEncryption,settings.Table,encryptionAlgorithm,settings.EncryptionKey);

            return error;
        }

        private async void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            bool error = false;
           
            error = AddTables(GetDatabasePath());
            
            if (error==true)
                MessageDialog.ShowMessage("No tables found or DB");
            else
            { 
                
                error = await Add();
                if (error==true)
                {
                    MessageDialog.ShowMessage("Failed to add record to database");
                }
                
            }
        }


        private void ButtonUpdate_OnClick(object? sender, RoutedEventArgs e)
        {
         
            
            bool error = false;
           
            error = AddTables(GetDatabasePath());
            
            if (error==true)
                Message.MessageDialog.ShowMessage("No tables found or DB");
            else
            {
                 Update();
                 
            }
            
            
            
            
            SaveSettings();
            
            
        }

        private async void ButtonDbPath_OnClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                this.Find<TextBox>("TextBoxDbPath").Text= await SettingsManagement.PatchDialog.GetFilePatch(this);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private async void ButtonCreateTable_OnClick(object? sender, RoutedEventArgs e)
        {
            
           string nameTable = await MessageDialog.DataInput();
           string dbPath = GetDatabasePath();
           if (nameTable!=default)
           {
               try
               {
                   DatabaseManagement databaseManagement = new DatabaseManagement(dbPath);
                   databaseManagement.CreateTable(nameTable);
                   databaseManagement.DbConnector.Close();
               }
               catch (Exception exception)
               {
                   MessageDialog.ShowMessage("failed to create table");
               }
               
               
           }
           else
           {
               MessageDialog.ShowMessage("failed to create table");
           }

           AddTables(dbPath);



        }

        private void ButtonDeleteTable_OnClick(object? sender, RoutedEventArgs e)
        {
            string nameTable = GetSettings().Table;
            string dbPath = GetDatabasePath();
            if (nameTable!="")
            {
                try
                {
                    DatabaseManagement databaseManagement = new DatabaseManagement(dbPath);
                    databaseManagement.DeleteTable(nameTable);
                    databaseManagement.DbConnector.Close();
                }
                catch (Exception exception)
                {
                    MessageDialog.ShowMessage("failed to Delete table");
                }
               
               
            }
            else
            {
                MessageDialog.ShowMessage("Unselected table");
            }
            AddTables(dbPath);
        }

        private void ComboBoxenEryptionAlgorithm_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            try
            {
                CryptoObj.EeryptAlgorithm algorithm =
                    (CryptoObj.EeryptAlgorithm) this.Find<ComboBox>("ComboBoxenEryptAlgorithm").SelectedIndex;

                if (algorithm == CryptoObj.EeryptAlgorithm.Des)
                {
                    this.Find<TextBox>("TextBoxKey").IsVisible = true;
                    this.Find<Button>("RsaSettingButton").IsVisible = false;
                }
                else if (algorithm == CryptoObj.EeryptAlgorithm.Rsa) 
                {
                    this.Find<TextBox>("TextBoxKey").IsVisible = false;
                    this.Find<Button>("RsaSettingButton").IsVisible = true;
                }
            }
            catch (Exception exception)
            {
               
            }
           
            

        }

        private void RsaSettingButton_OnClick(object? sender, RoutedEventArgs e)
        {
           ASINKkey.Show(this);
        }
    }
}

