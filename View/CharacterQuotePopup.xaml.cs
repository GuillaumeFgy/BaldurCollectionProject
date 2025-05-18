using MyApp.Model;

namespace MyApp.View;

public partial class CharacterQuotePopup : CommunityToolkit.Maui.Views.Popup
{
    public CharacterQuotePopup(BaldurCharacter character)
    {
        InitializeComponent();

        NameLabel.Text = character.Name;
        QuoteLabel.Text = $"« {character.Quote} »";
        PortraitImage.Source = character.Picture;
    }

    private void Close_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
