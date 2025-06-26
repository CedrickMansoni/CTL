

using ctl.mobile.viewmodel.Share.ViewModel;

namespace ctl.mobile.view.Cliente.View;

public partial class Reserva_ClientPage : ContentPage
{
	public Reserva_ClientPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Reservas_ViewModel)BindingContext;
		vm.ListarMarcacoesCommand.Execute(null);
	}
}