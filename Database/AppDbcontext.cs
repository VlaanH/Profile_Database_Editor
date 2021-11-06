using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Profile_Database_Editor.Data;

namespace Profile_Database_Editor.Database
{
    public enum EModels
    {
        UserData1,
        UserData2,
        UserData3
            
    }

    public class UserData1 : UserData { }
    public class UserData2 : UserData { }
    public class UserData3 : UserData { }
    
    
    
    public class AppDbContext :  DbContext
    {   
        //for easy creation of migrations
        public string _databasePath = "Profile_database.db";
        
        
        public DbSet<UserData1> userData1 { get; set; }
        public DbSet<UserData2> userData2 { get; set; }
        public DbSet<UserData3> userData3 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }
    }


}