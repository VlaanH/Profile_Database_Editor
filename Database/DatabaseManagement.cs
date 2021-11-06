using System;
using System.Collections.Generic;
using System.Linq;
using Profile_Database_Editor.Data;

namespace Profile_Database_Editor.Database
{
    public class DatabaseManagement
    {
        public string _databasePathg;

        public DatabaseManagement(string databasePathg)
        {
            _databasePathg = databasePathg;
        }

        public List<string> GetAllTables()
        {
            var allModels = Enum.GetNames(typeof(Database.EModels)).ToList();

            
            
            return allModels;
        }
        
        public void AddRecordDb(UserData userData, EModels model)
        {
           
            using ( AppDbContext appDbContext = new AppDbContext())
            {
                appDbContext._databasePath = _databasePathg;
                
                if (model == EModels.UserData1)
                {
                    appDbContext.userData1.Add(new UserData1()
                    {
                        Id = userData.Id,
        
                        UserName = userData.UserName,

                        Email =userData.Email,

                        Password = userData.Password

                    });
                }
                else if(model == EModels.UserData2)
                {
                    appDbContext.userData2.Add(new UserData2()
                    {
                        Id = userData.Id,
        
                        UserName = userData.UserName,

                        Email =userData.Email,

                        Password = userData.Password

                    });
                }
                else if(model == EModels.UserData3)
                {
                    appDbContext.userData3.Add(new UserData3()
                    {
                        Id = userData.Id,
        
                        UserName = userData.UserName,

                        Email =userData.Email,

                        Password = userData.Password

                    });
                }

                appDbContext.SaveChanges();
            }
            
           
        }

        public void UpdateRecordDb(UserData userData, EModels model)
        {
            using (AppDbContext appDbContext = new AppDbContext())
            {
                appDbContext._databasePath = _databasePathg;
                var updateFunc = new Func<UserData, bool>(user => user.Id == userData.Id);

                UserData updateUserdata = new UserData();


                if (model == EModels.UserData1)
                {
                    updateUserdata = appDbContext.userData1.Single(updateFunc);
                }
                else if (model == EModels.UserData2)
                {
                    updateUserdata = appDbContext.userData2.Single(updateFunc);
                }
                else if (model == EModels.UserData3)
                {
                    updateUserdata = appDbContext.userData3.Single(updateFunc);
                }


                updateUserdata.Id = userData.Id;

                updateUserdata.UserName = userData.UserName;

                updateUserdata.Email = userData.Email;

                updateUserdata.Password = userData.Password;


                appDbContext.SaveChanges();

            }


        }


        public List<UserData> GetAllRecords(EModels model)
        {
            using (AppDbContext appDbContext = new AppDbContext())
            {
                appDbContext._databasePath = _databasePathg;
                List<UserData> listUserData = new List<UserData>();

                if (model == EModels.UserData1)
                {
                    listUserData = new List<UserData>(appDbContext.userData1.ToList());
                }
                else if(model == EModels.UserData2)
                {
                    listUserData = new List<UserData>(appDbContext.userData2.ToList());
                }
                else if(model == EModels.UserData3)
                {
                    listUserData = new List<UserData>(appDbContext.userData3.ToList());
                }
                return listUserData;
            }
            
          
        }

        public void DeleteRecord(EModels model,int id)
        {
            using (AppDbContext appDbContext = new AppDbContext())
            {
                appDbContext._databasePath = _databasePathg;
                
                var linqFun = new Func<UserData, bool>(ud => ud.Id == id);

                UserData userData = new UserData();

                if (model == EModels.UserData1)
                {
                    appDbContext.Remove(appDbContext.userData1.Single(linqFun));
                    appDbContext.SaveChanges();
                }
                else if (model == EModels.UserData2)
                {
                    appDbContext.Remove(appDbContext.userData2.Single(linqFun));
                    appDbContext.SaveChanges();
                }
                else if (model == EModels.UserData3)
                {
                    appDbContext.Remove(appDbContext.userData3.Single(linqFun));
                    appDbContext.SaveChanges();
                }

            }
        }
        

        
        
        
    }
}
