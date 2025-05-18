using Microsoft.Maui.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyApp.Service;

public class JSONServices
{
    internal async Task<List<BaldurCharacter>> GetUserCharacters()
    {
        var url = "https://185.157.245.38:5000/json?FileName=BG3.json";

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        HttpClient _httpClient = new(handler);

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new List<BaldurCharacter>();

        var content = await response.Content.ReadAsStreamAsync();
        var allData = await JsonSerializer.DeserializeAsync<List<UserCharacterData>>(content) ?? new();

        var userId = Session.CurrentUser?.Id ?? "anonymous";
        return allData.FirstOrDefault(d => d.UserId == userId)?.Characters ?? new();
    }

    internal async Task SetUserCharacters(List<BaldurCharacter> updatedCharacters)
    {
        var url = "https://185.157.245.38:5000/json";
        var userId = Session.CurrentUser?.Id ?? "anonymous";

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        HttpClient _httpClient = new(handler);

        // 1. Télécharger tout le fichier
        var getResponse = await _httpClient.GetAsync($"{url}?FileName=BG3.json");

        List<UserCharacterData> allData = [];

        if (getResponse.IsSuccessStatusCode)
        {
            var content = await getResponse.Content.ReadAsStreamAsync();
            allData = await JsonSerializer.DeserializeAsync<List<UserCharacterData>>(content) ?? new();
        }

        // 2. Remplacer les données de l'utilisateur courant
        var existing = allData.FirstOrDefault(u => u.UserId == userId);
        if (existing != null)
            existing.Characters = updatedCharacters;
        else
            allData.Add(new UserCharacterData { UserId = userId, Characters = updatedCharacters });

        // 3. Envoyer tout le fichier modifié
        using var mystream = new MemoryStream();
        await JsonSerializer.SerializeAsync(mystream, allData);
        mystream.Position = 0;

        var fileContent = new ByteArrayContent(mystream.ToArray());
        var contentToSend = new MultipartFormDataContent
    {
        { fileContent, "file", "BG3.json" }
    };

        var postResponse = await _httpClient.PostAsync(url, contentToSend);
        if (postResponse.IsSuccessStatusCode)
        {
            await Application.Current.MainPage.DisplayAlert("Success", "Data saved successfully.", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to save JSON data.", "OK");
        }

    }
}
