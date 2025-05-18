using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyApp.Model;
using MyApp.Service;
using System.Collections.ObjectModel;

namespace MyApp.ViewModel;

[QueryProperty(nameof(UserId), "userId")]
public partial class UserViewModel : BaseViewModel
{
    private readonly UserService _userService;
    [ObservableProperty]
    public string? userId;
    
    public ObservableCollection<UserAccount> Users { get; } = new();

    [ObservableProperty] private string firstName;
    [ObservableProperty] private string lastName;
    [ObservableProperty] private string email;
    [ObservableProperty] private string password;

    public UserViewModel(UserService userService)
    {
        _userService = userService;
        LoadUsersCommand = new AsyncRelayCommand(LoadUsers);
        AddUserCommand = new AsyncRelayCommand(AddUser);
    }

    public IAsyncRelayCommand LoadUsersCommand { get; }
    public IAsyncRelayCommand AddUserCommand { get; }

    private async Task LoadUsers()
    {
        Users.Clear();
        var list = await _userService.GetUsersAsync();
        foreach (var user in list)
            Users.Add(user);
    }

    private async Task AddUser()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "All fields are mandatory.", "OK");
            return;
        }

        var user = new UserAccount
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            PasswordHash = Password
        };

        await _userService.CreateUserAsync(user);
        await LoadUsers();
        FirstName = LastName = Email = Password = string.Empty;
    }


}
