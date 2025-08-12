namespace test01.Data
{
    public class UserPermission
    {

        public int UserId { get; set; }
        public Permission PermissionId { get; set; }
        public User User { get; set; } = null!;
    }
}
