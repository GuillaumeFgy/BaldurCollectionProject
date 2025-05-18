using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyApp.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    public ObservableCollection<BaldurCharacter> MyObservableList { get; } = [];
    JSONServices MyJSONService;
    CSVServices MyCSVServices;

    public MainViewModel(JSONServices MyJSONService, CSVServices MyCSVServices)
    {
        this.MyJSONService = MyJSONService;
        this.MyCSVServices = MyCSVServices;
    }
 
    [RelayCommand]
    internal async Task GoToDetails(string id)
    {
        IsBusy = true;

        await Shell.Current.GoToAsync("DetailsView", true, new Dictionary<string,object>
        {
            {"selectedAnimal",id}
        });

        IsBusy = false;
    }

    [RelayCommand]
    internal async Task GoToUserPage()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync("UserView");
    }

    [RelayCommand]
    internal async Task EditUser(string userId)
    {
        await Shell.Current.GoToAsync($"UserView?userId={userId}");
    }

    [RelayCommand]
    internal async Task GoToGraph()
    {
        IsBusy = true;
        await Shell.Current.GoToAsync("GraphView", true);

        IsBusy = false;
    }
    [RelayCommand]
    internal async Task PrintToCSV()
    {
        IsBusy = true;

        await MyCSVServices.PrintData(Globals.MyBaldurCharacters);

        IsBusy = false;
    }

    [RelayCommand]
    internal async Task LoadFromCSV()
    {
        IsBusy = true;

        var newItems = await MyCSVServices.LoadData();

        bool addedAny = false;

        foreach (var newItem in newItems)
        {
            var existing = Globals.MyBaldurCharacters.FirstOrDefault(item => item.Id == newItem.Id);
            if (existing != null)
            {
                // Mise à jour des champs existants
                existing.Name = newItem.Name;
                existing.Picture = newItem.Picture;
                existing.Class = newItem.Class;
                existing.Race = newItem.Race;
                existing.Quote = newItem.Quote;
            }
            else
            {
                Globals.MyBaldurCharacters.Add(newItem);
                addedAny = true;
            }
        }


        await RefreshPage();

        if (addedAny)
        {
            await MyJSONService.SetUserCharacters(Globals.MyBaldurCharacters);
        }

        IsBusy = false;
    }



    [RelayCommand]
    internal async Task UploadJSON()
    {
        IsBusy = true;

        await MyJSONService.SetUserCharacters(Globals.MyBaldurCharacters);

        IsBusy = false;
    }


    internal async Task RefreshPage()
    {
        MyObservableList.Clear();

        if (Globals.MyBaldurCharacters.Count == 0)
            Globals.MyBaldurCharacters = await MyJSONService.GetUserCharacters();

        foreach (var item in Globals.MyBaldurCharacters)
        {
            MyObservableList.Add(item);
        }
    }

    public bool IsAdmin => Session.CurrentUser?.Role == "admin";

    [RelayCommand]
    internal async Task GoToAdminUsersPage()
    {
        if (Session.CurrentUser?.Role != "admin")
        {
            await Application.Current.MainPage.DisplayAlert("Access Denied", "Only administrators can access this page.", "OK");
            return;
        }

        await Shell.Current.GoToAsync(nameof(AdminUsersView));
    }


}
