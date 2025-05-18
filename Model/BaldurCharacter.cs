namespace MyApp.Model;
public class BaldurCharacter
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Race { get; set; } = string.Empty;
    public string Quote { get; set; } = "Je suis une énigme…";
    public string SafePicture
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Picture))
            {
                // Supposons que Picture contient juste le nom du fichier (ex. "portrait_elf.png")
                string resourcePath = $"Images/{Picture}";
               return resourcePath; // Utilisé avec .NET MAUI - intégré dans Resources/Images
            }

            return "Images/portrait_custom.png";
        }
    }


}