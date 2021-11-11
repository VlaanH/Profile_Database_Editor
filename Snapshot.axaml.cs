using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using Avalonia;
using Profile_Database_Editor.InterfaceObjects;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia.Enums;
using Profile_Database_Editor.DbSnapshot;
using Profile_Database_Editor.Message;

namespace Profile_Database_Editor
{
    public class Snapshot : Window
    {
        private TextBox _snapFile;
        private ScrollViewer _snapRecords;
        public Snapshot()
        {
            InitializeComponent();
            
            InitItemControl();
            InterfaceSnapshotRecord.SnapshotStackPanel = new StackPanel();
            _snapRecords!.Content = InterfaceSnapshotRecord.SnapshotStackPanel;
            
            SetSettings();
        }

        private void InitItemControl()
        {
            _snapFile = this.Find<TextBox>("SnapFile");
            _snapRecords = this.Find<ScrollViewer>("SnapRecords");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        
        }

        async void SetSettings()
        {
            //get the path to the file and leave only its name
            _snapFile.Text = new FileInfo((await Settings.SettingsManagement.Get()).DatabasePath).Name;
           WriteAllSnaps();
        }
        public static void Show(Window parent)
        {
            var msgbox = new Snapshot();
            msgbox.ShowDialog(parent);
        }

        async void WriteAllSnaps()
        {
            try
            {
                var dbPath=(await Settings.SettingsManagement.Get()).DatabasePath;
                var allSnaps=  SnapshotManagement.GetAllSnapshot(dbPath);
            
                InterfaceSnapshotRecord.SnapshotsOutput(allSnaps);
            }
            catch (Exception e)
            {
               
            }
           
        }

        private async void CreateSnapButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var snapName= await MessageDialog.DataInput("Snap Name", "Create Snapshot");
            if (snapName!=default)
            {
                var dbPath=(await Settings.SettingsManagement.Get()).DatabasePath;
                SnapshotManagement.CreateSnapshot(dbPath,snapName);
                WriteAllSnaps();
            }
           
        }

        private async void UseSnapshot_OnClick(object? sender, RoutedEventArgs e)
        {
          
            if (InterfaceSnapshotRecord.SelectedSnap!=default)
            {  
                var msRes = await MessageDialog.YesNo("Do you really want to use this snapshot?");
                if (msRes==ButtonResult.Yes)
                {  
                    var dbPath=(await Settings.SettingsManagement.Get()).DatabasePath;
                    
                    
                    SnapshotManagement.UseSnapshot(dbPath,InterfaceSnapshotRecord.SelectedSnap);
                } 
            }
            else
            {
                MessageDialog.ShowMessage("no snapshot selected");
            }
           

        }
    }
}