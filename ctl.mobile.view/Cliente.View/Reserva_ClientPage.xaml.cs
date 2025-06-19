

using ctl.mobile.viewmodel.Client.ViewModel;

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
		var vm = (Reserva_ViewModel)BindingContext;
		vm.ListarMarcacoesCommand.Execute(null);
    }
}