using CommunityToolkit.Maui.Views;

namespace MyApp.View;

public partial class MainView : ContentPage
{
	MainViewModel viewModel;
	public MainView(MainViewModel viewModel)
	{
		this.viewModel = viewModel;
		InitializeComponent();
		BindingContext=viewModel;
        Title = "Home version 2.0";
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Redirection si non connecté
        if (Session.CurrentUser == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please log in to access this page.", "OK");
            await Shell.Current.GoToAsync("///LoginView");
            return;
        }

        BindingContext = null;
        await viewModel.RefreshPage();
        BindingContext = viewModel;
    }

}