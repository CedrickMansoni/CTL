using ctl.mobile.view.Office.View;

namespace ctl.mobile.office;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	void RegisterRoutes()
	{
		Routing.RegisterRoute(nameof(Noticia_OfficePage), typeof(Noticia_OfficePage));
		Routing.RegisterRoute(nameof(Campo_OfficePageAdd), typeof(Campo_OfficePageAdd));
	}
}
