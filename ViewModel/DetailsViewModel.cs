using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using MyApp.View;

namespace MyApp.ViewModel;

[QueryProperty(nameof(Id), "selectedAnimal")]
public partial class DetailsViewModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Id { get; set; }
    [ObservableProperty]
    public partial string? Name { get; set; }
    [ObservableProperty]
    public partial string? Picture { get; set; }

    [ObservableProperty]
    public partial string? Class { get; set; }
    [ObservableProperty]
    public partial string? Race { get; set; }
    [ObservableProperty]
    public partial string? Quote { get; set; }

    [ObservableProperty]
    public partial string? SerialBufferContent { get; set; }
    [ObservableProperty]
    public partial bool EmulatorON_OFF { get; set; } = false;
    [ObservableProperty]
    private string manualScanId = string.Empty;

    readonly DeviceOrientationService MyScanner;

    [ObservableProperty]
    private string selectedPicture = "portrait_custom.png";



    IDispatcherTimer emulator = Application.Current.Dispatcher.CreateTimer();

    readonly MainViewModel _mainViewModel;
    private string? _lastNavigatedId;
    private string? _originalId;



    public DetailsViewModel(DeviceOrientationService myScanner, MainViewModel mainViewModel)
    {
        MyScanner = myScanner;
        _mainViewModel = mainViewModel;

        MyScanner.OpenPort();
        myScanner.SerialBuffer.Changed += OnSerialDataReception;
    }

    private async void OnSerialDataReception(object sender, EventArgs arg)
    {
        if (!EmulatorON_OFF) return;

        DeviceOrientationService.QueueBuffer MyLocalBuffer = (DeviceOrientationService.QueueBuffer)sender;

        if (MyLocalBuffer.Count > 0)
        {
            var incoming = MyLocalBuffer.Dequeue()?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(incoming))
            {
                SerialBufferContent += incoming;

                // Check if already navigated for this ID
                if (incoming == _lastNavigatedId)
                    return;

                var recognized = Globals.MyBaldurCharacters.FirstOrDefault(a => a.Id == incoming);
                if (recognized != null)
                {
                    _lastNavigatedId = incoming;
                    await Application.Current.Dispatcher.DispatchAsync(() =>
                    {
                        var popup = new CharacterQuotePopup(recognized);
                        Application.Current.MainPage.ShowPopup(popup);
                    });
                    await _mainViewModel.GoToDetails(recognized.Id);
                }
            }
        }
    }
    partial void OnManualScanIdChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            MyScanner.SerialBuffer.Enqueue(value.Trim());
        }
    }


    internal void RefreshPage()
    {
        foreach (var item in Globals.MyBaldurCharacters)
        {
            if (Id == item.Id)
            {
                _originalId = item.Id;
                Name = item.Name;
                Picture = item.Picture;
                Class = item.Class;
                Race = item.Race;
                Quote = item.Quote;
                SelectedPicture = item.Picture;
                break;
            }
        }
    }


    internal void ClosePage()
    {
        MyScanner.SerialBuffer.Changed -= OnSerialDataReception;
        MyScanner.ClosePort();       
    }

    [RelayCommand]
    internal async Task ChangeObjectParameters()
    {
        // Vérification de champs obligatoires
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Class) ||
            string.IsNullOrWhiteSpace(Race))
        {
            await Application.Current.MainPage.DisplayAlert("Missing Fields", "Please fill in all required fields (Name, Class, Race).", "OK");

            return;
        }

        var existing = Globals.MyBaldurCharacters.FirstOrDefault(item => item.Id == _originalId);

        if (existing != null)
        {
            existing.Id = Id ?? existing.Id;
            existing.Name = Name.Trim();
            existing.Picture = SelectedPicture ?? string.Empty;
            existing.Class = Class.Trim();
            existing.Race = Race.Trim();
            existing.Quote = Quote.Trim();
        }

        else
        {
            var newCharacter = new BaldurCharacter
            {
                Id = Id ?? Guid.NewGuid().ToString(),
                Name = Name.Trim(),
                Picture = string.IsNullOrWhiteSpace(Picture) ? "portrait_custom.png" : Picture.Trim(),
                Class = Class.Trim(),
                Race = Race.Trim(),
                Quote = Quote?.Trim() ?? string.Empty
            };


            Globals.MyBaldurCharacters.Add(newCharacter);
        }

        await Application.Current.MainPage.DisplayAlert("Success", "Changes have been saved.", "OK");

        // Enregistrer automatiquement sur le serveur
        await new JSONServices().SetUserCharacters(Globals.MyBaldurCharacters);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    public async Task OpenImagePopup()
    {
        var popup = new ImagePickerPopup();
        var result = await Application.Current.MainPage.ShowPopupAsync(popup);

        if (result is string selectedImage)
        {
            SelectedPicture = selectedImage;
            Picture = Path.GetFileName(selectedImage); // sauvegarde sans le "Images/"
        }
    }
}
