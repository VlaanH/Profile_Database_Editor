using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using Profile_Database_Editor.Data;

namespace Profile_Database_Editor.Database
{
    public class DatabaseManagement
    {
        public SQLiteConnection DbConnector;

        public DatabaseManagement(string databasePath)
        {
            DbConnector = new SQLiteConnection($"Data Source={databasePath}");
            DbConnector.Open();

        }
        public List<string> GetAllTables()
        {

            List<string> listTables = new List<string>();

            SQLiteCommand command  = DbConnector.CreateCommand();
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type = 'table'";
            SQLiteDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                listTables.Add(reader[0].ToString());
            }

               
          
            reader.Close();
            
            return listTables;
        }
        
        public void AddRecordDb(UserData userData, string nameTable)
        {
           
            SQLiteCommand command = DbConnector.CreateCommand();

            command.CommandText =
                    $"insert into '{nameTable}'(Email,UserName,Password) values ('{userData.Email}','{userData.UserName}','{userData.Password}')";
            
                
            command.ExecuteNonQuery();
            
        }

        public void UpdateRecordDb(UserData userData, string nameTable)
        {
          
            SQLiteCommand command = DbConnector.CreateCommand();

            command.CommandText =
                    $"UPDATE '{nameTable}' SET Email = '{userData.Email}',UserName = '{userData.UserName}',Password='{userData.Password}'  WHERE id = '{userData.Id}' ";

            command.ExecuteNonQuery();
    
        }
        public void CreateTable(string nameTable)
        {
           
            SQLiteCommand command = DbConnector.CreateCommand();

            command.CommandText =$"CREATE TABLE '{nameTable}' ('id'	INTEGER UNIQUE,'Email'	TEXT,'UserName'	TEXT,'Password'	TEXT,PRIMARY KEY('id'))";
             

            command.ExecuteNonQuery();
    
        }

        public void DeleteTable(string nameTable)
        {
            
            SQLiteCommand command = DbConnector.CreateCommand();

            command.CommandText =$"DROP TABLE IF EXISTS '{nameTable}'";
             

            command.ExecuteNonQuery();

        }

        
        
        public List<UserData> GetAllRecords(string nameTable)
        {
          
            List<UserData> listUserData = new List<UserData>();

            SQLiteCommand command  = DbConnector.CreateCommand();
            command.CommandText = $"SELECT * FROM '{nameTable}'";
            SQLiteDataReader reader = command.ExecuteReader();
            
                while (reader.Read())
                {
                    UserData userData = new UserData();
                    userData.Id = int.Parse(reader[0].ToString()!);
                    userData.Email = reader[1].ToString();
                    userData.UserName = reader[2].ToString();
                    userData.Password = reader[3].ToString();

                    listUserData.Add(userData);
                }

               
          
            reader.Close();
            return listUserData;
        }

        public void DeleteRecord(string nameTable,int id)
        {
            SQLiteCommand command = DbConnector.CreateCommand();

            command.CommandText =
                $"DELETE FROM '{nameTable}' WHERE id = {id};";
                
            
                
            command.ExecuteNonQuery();
        }
        

        
        
        
    }
}
