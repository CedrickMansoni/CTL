using ctl.mobile.viewmodel.Office.ViewModel;

namespace ctl.mobile.view.Cliente.View;

public partial class Banco_ClientPage : ContentPage
{
	public Banco_ClientPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Banco_MainPageViewModel)BindingContext;
		vm.ListarBancoCommand.Execute(null);
	}
}