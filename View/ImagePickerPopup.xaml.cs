using CommunityToolkit.Maui.Views;

namespace MyApp.View;

public partial class ImagePickerPopup : Popup
{
    public List<string> ImagePaths { get; } = new()
    {
        "portrait_custom.png",
        "shadow.jpg",
        "alfira.png",
        "astarion.jpg",
        "barcus.png",
        "darkurge.png",
        "gale.jpg",
        "halsin.png",
        "jaheira.png",
        "karlach.jpg",
        "laezel.jpg",
        "minsc.png",
        "minthara.jpg",
        "orin.jpg",
        "raphael.jpeg",
        "ravengard.png",
        "scratch.jpg",
        "thorm.jpg",
        "volo.png",
        "withers.png",
        "wyll.jpeg"
    };

    public ImagePickerPopup()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string selected)
        {
            Close(selected); // Renvoie le chemin complet "Images/..."
        }
    }
}
