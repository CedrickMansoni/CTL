
using ctl.mobile.viewmodel.Share.ViewModel;

namespace ctl.mobile.view.Office.View;

public partial class Reserva_OfficePage : ContentPage
{
	public Reserva_OfficePage()
	{
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Reservas_ViewModel)BindingContext;
		vm.ListarTodasMarcacoesCommand.Execute(null);
	}
}