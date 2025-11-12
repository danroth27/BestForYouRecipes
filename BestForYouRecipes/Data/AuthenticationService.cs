using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BestForYouRecipes.Data;

public class AuthenticationService
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly IUserStore _userStore;

    public AuthenticationService(ProtectedSessionStorage sessionStorage, IUserStore userStore)
    {
        _sessionStorage = sessionStorage;
        _userStore = userStore;
    }

    public async Task<bool> LoginAsync(string emailOrUsername, string password)
    {
        var user = await _userStore.GetUserByEmail(emailOrUsername) 
                   ?? await _userStore.GetUserByUsername(emailOrUsername);
        
        if (user == null)
        {
            return false;
        }

        var isValid = await _userStore.ValidatePassword(emailOrUsername, password);
        
        if (isValid)
        {
            await _sessionStorage.SetAsync("user", new UserSession
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName
            });
            return true;
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await _sessionStorage.DeleteAsync("user");
    }

    public async Task<UserSession?> GetCurrentUserAsync()
    {
        try
        {
            var result = await _sessionStorage.GetAsync<UserSession>("user");
            return result.Success ? result.Value : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetCurrentUserAsync();
        return user != null;
    }
}

public class UserSession
{
    public string UserId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? FullName { get; set; }
}

