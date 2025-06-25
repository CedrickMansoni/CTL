using ctl.mobile.view.Office.View;
using ctl.mobile.view.Share.View;

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
		Routing.RegisterRoute(nameof(Banco_OfficePageAdd), typeof(Banco_OfficePageAdd));
		Routing.RegisterRoute(nameof(NoticiaDetalhePage), typeof(NoticiaDetalhePage));
		Routing.RegisterRoute("NoticiaEdite_OfficePage", typeof(NoticiaEdite_OfficePage));
		Routing.RegisterRoute("Campo_OfficePageEdite", typeof(Campo_OfficePageEdite));
	}
}
