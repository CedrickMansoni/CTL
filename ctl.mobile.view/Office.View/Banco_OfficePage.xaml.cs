using ctl.mobile.viewmodel.Office.ViewModel;

namespace ctl.mobile.view.Office.View;

public partial class Banco_OfficePage : ContentPage
{
	public Banco_OfficePage()
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