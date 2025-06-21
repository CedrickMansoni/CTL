using ctl.mobile.view.Cliente.View;
using ctl.mobile.view.Share.View;

namespace ctl.mobile.cliente;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegistrarRotas();
	}

	void RegistrarRotas()
	{
		Routing.RegisterRoute(nameof(Conta_ClientPage), typeof(Conta_ClientPage));
		Routing.RegisterRoute(nameof(Senha_ClientPage), typeof(Senha_ClientPage));
		Routing.RegisterRoute(nameof(Campo_AgendamentoPage), typeof(Campo_AgendamentoPage));
		Routing.RegisterRoute(nameof(NoticiaDetalhePage), typeof(NoticiaDetalhePage));
	}
}
