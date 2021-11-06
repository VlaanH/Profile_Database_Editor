using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Profile_Database_Editor.DbSnapshot
{
    public class SnapshotManagement
    {
        private string GetSnapshotDirectory(string filePath)
        {
            return new FileInfo(filePath).Name+"_Snap";
        }

        public void CreateSnapshot(string dbPath)
        {
            string dateNow = DateTime.Now.ToString();

            string dbSnapshotDirectory = GetSnapshotDirectory(dbPath);
            
            
            if (Directory.Exists(dbPath)==false)
                Directory.CreateDirectory(dbSnapshotDirectory);
           
            
            File.Copy(dbPath,dbSnapshotDirectory+"/"+dateNow);

        }

        public void UseSnapshot(string dbPath,string snapshotName)
        {
            string dbSnapshotPath = GetSnapshotDirectory(dbPath)+"/"+snapshotName;
            
   
            
            File.Copy(dbSnapshotPath,dbPath,true);
            
      
        }

        public List<string> GetAllSnapshot(string dbPath)
        {
            string dbSnapshotPath = GetSnapshotDirectory(dbPath);
            var allSnap= Directory.GetFiles(dbSnapshotPath);
            
            return allSnap.ToList();
        }



    }
}