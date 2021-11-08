using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;

namespace Profile_Database_Editor.Message
{
    public static class MessageDialog
    {
        public static void ShowMessage(string message)
        {
            var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Eror",
                    ContentMessage = message,
                    Icon = Icon.Error,
                    Style = Style.UbuntuLinux,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                    
                });
        
            msBoxStandardWindow.Show();
            
        }

        public static async Task<ButtonResult> YesNo(string message)
        {
            var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams{
                    ButtonDefinitions = ButtonEnum.YesNo,
                    ContentMessage = message,
                    Icon = Icon.Stopwatch,
                    Style = Style.UbuntuLinux,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                    
                });
            return await msBoxStandardWindow.Show();

        }
        
        
        
        public static async Task<string> DataInput()
        {
            var messageBoxInputWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxInputWindow(new MessageBoxInputParams
                {
                    Style = Style.UbuntuLinux,
                    Topmost = true,
                    ShowInCenter = true,
                    ContentMessage = "Table name",
                    ContentTitle = "Enter table name",
                 
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Cancel", IsCancel = true },
                        new ButtonDefinition { Name = "Confirm", Type = ButtonType.Colored, IsDefault = true }
                    },
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                  

                    
                });
              var result= await messageBoxInputWindow.Show();
             
              return result.Message;

        }



    }
    
    
    
}