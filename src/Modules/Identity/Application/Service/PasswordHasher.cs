namespace ELearning.Identity.Auth;
public class BCryptPasswordHasher
{
    public string HashPassword(string Password)
    {
        return BCrypt.Net.BCrypt.HashPassword(Password);   
    }
    public bool VerfiyPassword(string password,string HashedPassword)
    {
                return BCrypt.Net.BCrypt.Verify(password, HashedPassword); 
    }
}