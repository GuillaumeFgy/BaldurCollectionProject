using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyApp.Service;
using System.Threading.Tasks;

namespace MyApp.ViewModel;

public partial class LoginViewModel : BaseViewModel
{
    private readonly UserService _userService;

    [ObservableProperty] private string email;
    [ObservableProperty] private string password;

    public LoginViewModel(UserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        bool isValid = await _userService.ValidateCredentialsAsync(Email, Password);

        if (isValid)
        {
            Session.CurrentUser = await _userService.GetUserByEmailAsync(Email);
            Globals.MyBaldurCharacters.Clear();
            await Shell.Current.GoToAsync("///MainView"); // absolu


        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Failed", "Incorrect email or password.", "OK");

        }
    }

    [RelayCommand]
    private async Task GoToRegister()
    {
        await Shell.Current.GoToAsync(nameof(UserView));
    }
}
