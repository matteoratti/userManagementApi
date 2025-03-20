public class User
{
    public int Id { get; set; }
    required public string Username { get; set; }
    public string Email { get; set; } = string.Empty;
}