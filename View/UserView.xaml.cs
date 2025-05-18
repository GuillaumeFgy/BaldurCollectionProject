using MyApp.Service;
using MyApp.ViewModel;

namespace MyApp.View;

public partial class UserView : ContentPage
{
    public UserView()
    {
        InitializeComponent();
        BindingContext = App.Current?.Handler?.MauiContext?.Services?.GetService<UserViewModel>();
    }
}

