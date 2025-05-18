using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyApp.Model;

public class UserAccount
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;  // crypté
    public string Role { get; set; } = "user"; // ou admin
}
