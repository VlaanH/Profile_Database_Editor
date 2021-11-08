using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.Enums;
using Profile_Database_Editor.DbSnapshot;
using Profile_Database_Editor.Message;

namespace Profile_Database_Editor.InterfaceObjects
{
    public class InterfaceSnapshotRecord
    {
        public static string SelectedSnap = default;
        public static StackPanel SnapshotStackPanel = new StackPanel();
        public static List<Button> SnapButtonList = new List<Button>();

        private static void SelectSnap(Button selectedButton)
        {
            foreach (var button in SnapButtonList)
            {
                button.IsEnabled = true;
                if (button==selectedButton)
                {
                    button.IsEnabled = false;
                    SelectedSnap = button.Content.ToString();
                }
            }
            
        }

        public  static Grid GetSnapshotGrid(string snapName)
        {
            Grid snap = new Grid();
            
            snap.ColumnDefinitions.Add( new ColumnDefinition());
            snap.ColumnDefinitions.Add( new ColumnDefinition());
       
        
            snap.ColumnDefinitions[1].Width = GridLength.Parse("90");


            Button snapshotButton = new Button();
            snapshotButton.Content = snapName;
            Grid.SetColumn(snapshotButton,0);
            snap.Children.Add(snapshotButton);
            snapshotButton.Click += (s, e) =>
            {
                SelectSnap(snapshotButton);
            };
            
            
            Button dellRecordButton = new Button();
            dellRecordButton.Content = "-";
            Grid.SetColumn(dellRecordButton,1);
            snap.Children.Add(dellRecordButton);
            
            dellRecordButton.Click+= async (s, e) =>
            {

                var msboxRes= await MessageDialog.YesNo("delete this snapshot: " + snapName);
                if (msboxRes == ButtonResult.Yes )
                {
                    SnapshotStackPanel.Children.Remove(snap);
                    SnapButtonList.Remove(dellRecordButton);
                    SelectedSnap = default;
                    SnapshotManagement.DeleteSnapshot(snapName);
                }
                
                
            };
            
            SnapButtonList.Add(snapshotButton);
            return snap;
        }

        public async static void AddSnap(string snapName)
        {
            SnapshotStackPanel.Children.Add( GetSnapshotGrid(snapName));
        }

        public static void SnapshotsOutput(List<string> allSnapName)
        {
            SnapButtonList = new List<Button>();
            SnapshotStackPanel.Children.Clear();
            SelectedSnap = default;
            
            
            foreach (var name in allSnapName)
            {
                AddSnap(name);
            }
            
        }
        
        
    }
}