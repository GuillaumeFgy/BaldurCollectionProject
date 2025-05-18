using CommunityToolkit.Maui.Storage;

namespace MyApp.Service;

public class CSVServices
{
    public async Task<List<BaldurCharacter>> LoadData()
    {
        List<BaldurCharacter> list = [];

        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select a CSV file"
        });

        if (result == null)
        {
            await Application.Current.MainPage.DisplayAlert("Info", "No file selected.", "OK");
            return list;
        }

        try
        {
            using var stream = new FileStream(result.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            var lines = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }

            if (lines.Count < 2)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The selected file is empty or contains no data.", "OK");
                return list;
            }

            // Check header
            var headers = lines[0].Split(';');
            if (!headers.SequenceEqual(new[] { "Id", "Name", "Picture", "Class", "Race", "Quote" }))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The CSV file format is invalid.", "OK");
                return list;
            }

            for (int i = 1; i < lines.Count; i++)
            {
                var values = lines[i].Split(';');

                if (values.Length < 6)
                    continue; // Incomplete line

                var obj = new BaldurCharacter
                {
                    Id = values[0],
                    Name = values[1],
                    Picture = values[2],
                    Class = values[3],
                    Race = values[4],
                    Quote = values[5]
                };

                list.Add(obj);
            }
        }
        catch (IOException)
        {
            await Application.Current.MainPage.DisplayAlert("Read Error", "The file is open in another program. Please close it and try again.", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Unable to read the file: {ex.Message}", "OK");
        }

        return list;
    }

    public async Task PrintData(List<BaldurCharacter> data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Id;Name;Picture;Class;Race;Quote");

        foreach (var item in data)
        {
            var line = string.Join(";", new[]
            {
                item.Id,
                item.Name,
                item.Picture,
                item.Class,
                item.Race,
                item.Quote
            });

            csv.AppendLine(line);
        }

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString()));
        var fileSaverResult = await FileSaver.Default.SaveAsync("Collection.csv", stream);

        if (!fileSaverResult.IsSuccessful)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Unable to save the file.", "OK");
        }
    }
}

