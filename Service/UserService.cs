using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using MyApp.Model;

namespace MyApp.Service;

public class UserService
{
    private readonly IMongoCollection<UserAccount> _users;

    public UserService()
    {
        var client = new MongoClient("mongodb://student:IAmTh3B3st@185.157.245.38:5003");
        var database = client.GetDatabase("MyAppDBBG3");
        _users = database.GetCollection<UserAccount>("Users");
    }

    public async Task<List<UserAccount>> GetUsersAsync() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<UserAccount?> GetUserByEmailAsync(string email) =>
        await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task CreateUserAsync(UserAccount user)
    {
        user.PasswordHash = HashPassword(user.PasswordHash);
        await _users.InsertOneAsync(user);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        return user != null && VerifyPassword(password, user.PasswordHash);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}
