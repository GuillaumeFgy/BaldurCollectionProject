using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.ViewModel;

[QueryProperty(nameof(Id), "selectedAnimal")]
public partial class DetailsViewModel: ObservableObject
{
    [ObservableProperty]
    public partial string? Id { get; set; }
    [ObservableProperty]
    public partial string? Name { get; set; }
    [ObservableProperty]
    public partial string? Description { get; set; }
    [ObservableProperty]
    public partial string? Picture { get; set; }
    [ObservableProperty]
    public partial string? Class { get; set; }
    [ObservableProperty]
    public partial string? Race { get; set; }
    [ObservableProperty]
    public partial string? Faction { get; set; }
    [ObservableProperty]
    public partial string? SerialBufferContent { get; set; }
    [ObservableProperty]
    public partial bool EmulatorON_OFF { get; set; } = false;
    
    readonly DeviceOrientationService MyScanner;

    IDispatcherTimer emulator= Application.Current.Dispatcher.CreateTimer();

    readonly MainViewModel _mainViewModel;
    private string? _lastNavigatedId;


    public DetailsViewModel(DeviceOrientationService myScanner, MainViewModel mainViewModel)
    {
        MyScanner = myScanner;
        _mainViewModel = mainViewModel;

        MyScanner.OpenPort();
        myScanner.SerialBuffer.Changed += OnSerialDataReception;

        emulator.Interval = TimeSpan.FromSeconds(1);
        emulator.Tick += (s, e) => AddCode();
    }

    partial void OnEmulatorON_OFFChanged(bool value)
    {
        if (value)
        {
            _lastNavigatedId = null;
            emulator.Start();
        }
        else
        {
            emulator.Stop();
        }
    }

    private void AddCode()
    {
        MyScanner.SerialBuffer.Enqueue("1");
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
                OnPropertyChanged(nameof(SerialBufferContent));

                // Check if already navigated for this ID
                if (incoming == _lastNavigatedId)
                    return;

                var recognized = Globals.MyBaldurCharacters.FirstOrDefault(a => a.Id == incoming);
                if (recognized != null)
                {
                    _lastNavigatedId = incoming;
                    await _mainViewModel.GoToDetails(recognized.Id);
                }
            }
        }
    }


    internal void RefreshPage()
    {
        foreach (var item in Globals.MyBaldurCharacters)
        {
            if (Id == item.Id)
            {
                Name = item.Name;
                Description = item.Description;
                Picture = item.Picture;
                Class = item.Class;
                Race = item.Race;
                Faction = item.Faction;
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
        var existing = Globals.MyBaldurCharacters.FirstOrDefault(item => item.Id == Id);

        if (existing != null)
        {
            existing.Name = Name ?? string.Empty;
            existing.Description = Description ?? string.Empty;
            existing.Picture = Picture ?? string.Empty;
            existing.Class = Class ?? string.Empty;
            existing.Race = Race ?? string.Empty;
            existing.Faction = Faction ?? string.Empty;

        }
        else
        {
            var newCharacter = new BaldurCharacter
            {
                Id = Id ?? Guid.NewGuid().ToString(), 
                Name = Name ?? string.Empty,
                Description = Description ?? string.Empty,
                Picture = Picture ?? string.Empty,
                Class = Class ?? string.Empty,
                Race = Race ?? string.Empty,
                Faction = Faction ?? string.Empty
            };

            Globals.MyBaldurCharacters.Add(newCharacter);
        }

        await Application.Current.MainPage.DisplayAlert("Success", "Changes have been applied.", "OK");
        await Shell.Current.GoToAsync("..");
    }

}
