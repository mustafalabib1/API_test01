namespace test01.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICollection<UserPermission> Permissions { get; set; } = new List<UserPermission>();
    }
}
