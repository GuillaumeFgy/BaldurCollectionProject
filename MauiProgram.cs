using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace MyApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("CinzelDecorative-Regular.ttf", "CinzelDecorative");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<MainView>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<DetailsView>();
            builder.Services.AddTransient<DetailsViewModel>(sp =>
    new DetailsViewModel(sp.GetRequiredService<DeviceOrientationService>(), sp.GetRequiredService<MainViewModel>()));


            builder.Services.AddTransient<GraphView>();
            builder.Services.AddTransient<GraphViewModel>();

            builder.Services.AddSingleton<DeviceOrientationService>();
            builder.Services.AddSingleton<JSONServices>();
            builder.Services.AddSingleton<CSVServices>();

            builder.Services.AddSingleton<UserService>();
            builder.Services.AddTransient<UserView>();
            builder.Services.AddTransient<UserViewModel>();

            builder.Services.AddSingleton<SerialScannerService>();

            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<AdminUsersView>();
            builder.Services.AddTransient<AdminUsersViewModel>();

            return builder.Build();
        }
    }
}
