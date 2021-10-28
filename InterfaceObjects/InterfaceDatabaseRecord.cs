using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Avalonia.Controls;
using Profile_Database_Editor.Cryptography;
using Profile_Database_Editor.Data;
using Profile_Database_Editor.Database;
using Profile_Database_Editor.Message;

namespace Profile_Database_Editor.InterfaceObjects
{
    public static class InterfaceDatabaseRecord
    {
        
        public static StackPanel MainStackPanel = new StackPanel();
        public static Grid GetRecordGrid(UserData userData,DatabaseManagement databaseManagement,bool useEncryption,string encryptionKey,string table,CryptoObj.EeryptAlgorithm algorithm)
        {
            Grid record = new Grid();
            
            record.ColumnDefinitions.Add( new ColumnDefinition());
            record.ColumnDefinitions.Add( new ColumnDefinition());
            record.ColumnDefinitions.Add( new ColumnDefinition());
            record.ColumnDefinitions.Add( new ColumnDefinition());
            record.ColumnDefinitions.Add( new ColumnDefinition());
            record.ColumnDefinitions[4].Width = GridLength.Parse("90");
            record.ColumnDefinitions[0].Width = GridLength.Parse("90");

            bool isActivated = false;
            
            TextBox id = new TextBox();
            id.Classes = Classes.Parse("records");
            id.Text = userData.Id.ToString();
            record.Children.Add(id);
            id.IsEnabled = false;
            Grid.SetColumn(id,0);
            
            
            TextBox email = new TextBox();
            email.Classes = Classes.Parse("records");
            email.Text = userData.Email;
            record.Children.Add(email);
            email.IsEnabled = false;
            Grid.SetColumn(email,1);
            
            
            
            
            TextBox user = new TextBox();
            user.Classes = Classes.Parse("records");
            user.Text = userData.UserName;
            record.Children.Add(user);
            user.IsEnabled = false;
            Grid.SetColumn(user,2);
    
           
            
            
            
            TextBox password = new TextBox();
            password.Classes = Classes.Parse("records");
            password.Text = userData.Password;
            record.Children.Add(password);
            password.IsEnabled = false;
            Grid.SetColumn(password,3);



            Grid buttons = new Grid();
            buttons.ColumnDefinitions.Add( new ColumnDefinition());
            buttons.ColumnDefinitions.Add( new ColumnDefinition());

            Button recordUpdateButton = new Button();
            recordUpdateButton.Content = "↻";
            buttons.Children.Add(recordUpdateButton);
            Grid.SetColumn(recordUpdateButton,0);
            
            Button dellRecordButton = new Button();
            dellRecordButton.Content = "-";
            buttons.Children.Add(dellRecordButton);
            Grid.SetColumn(dellRecordButton,1);

         
            buttons.IsEnabled = userData.IsCorrectData;
           
                
            
            dellRecordButton.Click += (s, e) =>
            {
                databaseManagement.DeleteRecord(table,int.Parse(id.Text.ToString()));
                MainStackPanel.Children.Remove(record);
             
            };
            
            recordUpdateButton.Click += (s, e) =>
            {
                bool error = false;
                
                if (isActivated==false)
                {
                    user.IsEnabled = true;
                    email.IsEnabled = true;
                    password.IsEnabled = true;
                    isActivated = true;
                    recordUpdateButton.Content = "✓";
                }
                else
                {
                    UserData userData = new UserData()
                    {
                        UserName = user.Text,
                        Email = email.Text,
                        Id = int.Parse(id.Text),
                        Password = password.Text
                        
                    };
                    
                    if (useEncryption == true)
                    {
                        try
                        {
                            if (algorithm == CryptoObj.EeryptAlgorithm.Des)
                            {
                                userData = new Cryptography.DesCrypto(encryptionKey).EncryptionUserData(userData);  
                            }
                            else if (algorithm == CryptoObj.EeryptAlgorithm.Rsa)
                            {
                                userData = new Cryptography.RsaCrypto(encryptionKey,default).EncryptionUserData(userData);  
                            }
                        }
                        catch (Exception exception)
                        { 
                            
                            error = true;
                          MessageDialog.ShowMessage("data update error");
                        }
                       

                        
                    }

                  
                    
                    if(error==false)
                        databaseManagement.UpdateRecordDb(userData,table);
                    
                    user.IsEnabled = false;
                    email.IsEnabled = false;
                    password.IsEnabled = false;
                    isActivated = false;
                    recordUpdateButton.Content = "↻";
                }
            };
            
            record.Children.Add(buttons);
            
            Grid.SetColumn(buttons,4);
            return record;
        }

        public static void DatabaseOutput(List<UserData> listUserData,DatabaseManagement databaseManagement,bool useEncryption,string table,CryptoObj.EeryptAlgorithm encryptionAlgorithm,string encryptionKey = default)
        {
            MainStackPanel.Children.Clear();
            for (int i = 0; i < listUserData.Count; i++)
            {

               Grid recordGrid = GetRecordGrid(listUserData[i],databaseManagement,useEncryption,encryptionKey,table,encryptionAlgorithm);
               MainStackPanel.Children.Add(recordGrid);
               
            }
            
            
        }
    }
}