namespace BestForYouRecipes.Data;

public interface IUserStore
{
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByUsername(string username);
    Task<User> CreateUser(User user);
    Task<bool> ValidatePassword(string emailOrUsername, string password);
}

