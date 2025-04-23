using Microsoft.Extensions.Logging;

namespace ctl.mobile.cliente;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

				fonts.AddFont("Montserrat-Black.ttf", "MontserratBlack");
				fonts.AddFont("Montserrat-ExtraBold.ttf", "MontserratBold");
				fonts.AddFont("Montserrat-ExtraLight.ttf", "MontserratExtraLight");
				fonts.AddFont("Montserrat-Light.ttf", "MontserratLight");
				fonts.AddFont("Montserrat-Medium.ttf", "MontserratMedium");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
