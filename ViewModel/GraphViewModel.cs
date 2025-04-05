using Microcharts;
using Microsoft.Maui.Graphics;
using SkiaSharp;
using CommunityToolkit.Mvvm.ComponentModel;
using MyApp.Model;

namespace MyApp.ViewModel;

public partial class GraphViewModel : ObservableObject
{
    [ObservableProperty]
    public Chart myObservableChart;

    public GraphViewModel()
    {
        RefreshPage();
    }

    internal void RefreshPage()
    {
        if (Globals.MyBaldurCharacters.Count == 0)
        {
            myObservableChart = null!;
            return;
        }

        var grouped = Globals.MyBaldurCharacters
            .GroupBy(c => string.IsNullOrWhiteSpace(c.Faction) ? "Unknown" : c.Faction)
            .Select(g => new ChartEntry(g.Count())
            {
                Label = g.Key,
                ValueLabel = g.Count().ToString(),
                Color = SKColor.Parse(GenerateRandomColorHex())
            }).ToList();

        MyObservableChart = new PieChart
        {
            Entries = grouped,
            LabelTextSize = 32,
            HoleRadius = 0.4f,
            IsAnimated = true,
            BackgroundColor = SKColors.White
        };
    }

    private string GenerateRandomColorHex()
    {
        var random = new Random();
        return $"#{random.Next(0x1000000):X6}";
    }
}
