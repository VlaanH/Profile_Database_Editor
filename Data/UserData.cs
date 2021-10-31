namespace Profile_Database_Editor.Data
{
    public class UserData
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; } 

        public string Password { get; set; } 

        public bool IsCorrectData = true;
        
    }
}