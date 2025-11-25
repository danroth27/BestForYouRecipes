using System.Security.Cryptography;
using System.Text;

namespace BestForYouRecipes.Data;

public class UserStore : IUserStore
{
    private readonly Dictionary<string, User> usersByEmail = new();
    private readonly Dictionary<string, User> usersByUsername = new();
    private int nextUserId = 1;

    public Task<User?> GetUserByEmail(string email)
    {
        usersByEmail.TryGetValue(email.ToLowerInvariant(), out var user);
        return Task.FromResult(user);
    }

    public Task<User?> GetUserByUsername(string username)
    {
        usersByUsername.TryGetValue(username.ToLowerInvariant(), out var user);
        return Task.FromResult(user);
    }

    public Task<User> CreateUser(User user)
    {
        user.Id = nextUserId.ToString();
        nextUserId++;
        
        // Hash the password (assumes PasswordHash contains plain text password)
        user.PasswordHash = HashPassword(user.PasswordHash);
        
        var emailKey = user.Email.ToLowerInvariant();
        var usernameKey = user.Username.ToLowerInvariant();
        
        if (usersByEmail.ContainsKey(emailKey) || usersByUsername.ContainsKey(usernameKey))
        {
            throw new InvalidOperationException("User with this email or username already exists");
        }
        
        usersByEmail[emailKey] = user;
        usersByUsername[usernameKey] = user;
        
        return Task.FromResult(user);
    }

    public Task<bool> ValidatePassword(string emailOrUsername, string password)
    {
        var lowerKey = emailOrUsername.ToLowerInvariant();
        User? user = null;
        
        if (usersByEmail.TryGetValue(lowerKey, out user) || 
            usersByUsername.TryGetValue(lowerKey, out user))
        {
            var passwordHash = HashPassword(password);
            return Task.FromResult(user.PasswordHash == passwordHash);
        }
        
        return Task.FromResult(false);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public static string HashPasswordStatic(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

