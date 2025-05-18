using Microcharts;
using SkiaSharp;
using CommunityToolkit.Mvvm.ComponentModel;
using MyApp.Model;

namespace MyApp.ViewModel;

public partial class GraphViewModel : ObservableObject
{
    [ObservableProperty]
    public Chart classChart;

    [ObservableProperty]
    public Chart raceChart;

    public GraphViewModel()
    {
        RefreshPage();
    }

    internal void RefreshPage()
    {
        if (Globals.MyBaldurCharacters.Count == 0)
        {
            ClassChart = null!;
            RaceChart = null!;
            return;
        }

        // Graphique des classes
        var classEntries = Globals.MyBaldurCharacters
            .GroupBy(c => string.IsNullOrWhiteSpace(c.Class) ? "Inconnue" : c.Class)
            .Select(g => new ChartEntry(g.Count())
            {
                Label = g.Key,
                ValueLabel = g.Count().ToString(),
                Color = SKColor.Parse(GenerateRandomColorHex())
            }).ToList();

        ClassChart = new PieChart
        {
            Entries = classEntries,
            LabelTextSize = 32,
            HoleRadius = 0.4f,
            IsAnimated = true,
            BackgroundColor = SKColors.White
        };

        // Graphique des races
        var raceEntries = Globals.MyBaldurCharacters
            .GroupBy(c => string.IsNullOrWhiteSpace(c.Race) ? "Inconnue" : c.Race)
            .Select(g => new ChartEntry(g.Count())
            {
                Label = g.Key,
                ValueLabel = g.Count().ToString(),
                Color = SKColor.Parse(GenerateRandomColorHex())
            }).ToList();

        RaceChart = new PieChart
        {
            Entries = raceEntries,
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
