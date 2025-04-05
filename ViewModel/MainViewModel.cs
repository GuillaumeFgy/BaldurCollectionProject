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

        foreach (var newItem in newItems)
        {
            bool alreadyExists = Globals.MyBaldurCharacters.Any(item => item.Id == newItem.Id);
            if (!alreadyExists)
            {
                Globals.MyBaldurCharacters.Add(newItem);
            }
        }

        await RefreshPage();

        IsBusy = false;
    }

    [RelayCommand]
    internal async Task UploadJSON()
    {
        IsBusy = true;
  

        await MyJSONService.SetStrangeAnimals(Globals.MyBaldurCharacters);
        IsBusy = false;
    }
    
    internal async Task RefreshPage()
    {
        MyObservableList.Clear ();

        if(Globals.MyBaldurCharacters.Count == 0) Globals.MyBaldurCharacters = await MyJSONService.GetStrangeAnimals();

        foreach (var item in Globals.MyBaldurCharacters)
        {
            MyObservableList.Add(item);
        }
    }
}
