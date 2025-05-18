using MyApp.ViewModel;

namespace MyApp.View;

public partial class AdminUsersView : ContentPage
{
    public AdminUsersView()
    {
        InitializeComponent();
        BindingContext = App.Current?.Handler?.MauiContext?.Services?.GetService<AdminUsersViewModel>();
    }
}
