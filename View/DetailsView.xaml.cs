using MyApp.ViewModel;

namespace MyApp.View;

public partial class DetailsView : ContentPage
{
    readonly DetailsViewModel viewModel;
	public DetailsView(DetailsViewModel viewModel)
	{
		this.viewModel = viewModel;
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        BindingContext = null;
        viewModel.RefreshPage();    // Réinitialise la observablecollection
        BindingContext = viewModel;
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.ClosePage();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = string.Empty;
        }
    }

    private async void MyAnimatedButton_Clicked(object sender, EventArgs e)
    {
        await MyAnimatedButton.ScaleTo(1.1, 100);
        await MyAnimatedButton.ScaleTo(1.0, 100);
    }

}