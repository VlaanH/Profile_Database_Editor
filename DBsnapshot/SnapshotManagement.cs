using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Profile_Database_Editor.DbSnapshot
{
    public static class SnapshotManagement
    {
        private static string GetSnapshotDirectory(string filePath)
        {
            return new FileInfo(filePath).Name+"_Snap";
        }

        public static void CreateSnapshot(string dbPath,string snapshotName)
        {
            string dateNow = DateTime.Now.ToString();

            string dbSnapshotDirectory = GetSnapshotDirectory(dbPath);
            
            
            if (Directory.Exists(dbPath)==false)
                Directory.CreateDirectory(dbSnapshotDirectory);
           
            
            File.Copy(dbPath,dbSnapshotDirectory+"/"+dateNow+" | "+snapshotName);

        }

        public static void DeleteSnapshot(string snapshotPath)
        {
            File.Delete(snapshotPath);
        }

        public static void UseSnapshot(string dbPath,string snapshotPath)
        {
            
            
            File.Copy(snapshotPath,dbPath,true);
            
      
        }

        public static List<string> GetAllSnapshot(string dbPath)
        {
            string dbSnapshotPath = GetSnapshotDirectory(dbPath);
            var allSnap= Directory.GetFiles(dbSnapshotPath);
            
            return allSnap.ToList();
        }



    }
}