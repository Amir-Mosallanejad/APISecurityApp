namespace APISecurityApp.Model
{
    public class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool? IsAdmin { get; set; } = false;
    }
}
