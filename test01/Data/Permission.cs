namespace test01.Data
{
    [Flags]
    public enum Permission
    {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,
        Create = 16,
        Update = 32,
        Admin = 64
    }
}
