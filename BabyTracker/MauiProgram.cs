using BabyTracker.Services;
using BabyTracker.Views;
using Microsoft.Extensions.Logging;

namespace BabyTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                // Using system default fonts
            });

        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<HistoryPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
