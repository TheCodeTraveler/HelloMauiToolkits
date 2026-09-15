using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;

namespace HelloMauiToolkits;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.UseMauiCommunityToolkitMarkup()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});
#if DEBUG
		builder.Logging.AddDebug();
#endif
		// Add Pages + ViewModels
		builder.Services.AddTransientWithShellRoute<TapGamePage, TapGameViewModel>("/TapGamePage");

		// Add Services
		builder.Services.AddSingleton<App>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<TapCountService>();
		builder.Services.AddSingleton<IPreferences>(Preferences.Default);

		return builder.Build();
	}
}