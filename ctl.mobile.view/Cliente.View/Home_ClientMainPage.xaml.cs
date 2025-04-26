using ctl.mobile.viewmodel.Office.ViewModel;

namespace ctl.mobile.view.Cliente.View;

public partial class Home_ClientMainPage : ContentPage
{
	public Home_ClientMainPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Home_OfficeViewModel)BindingContext;
		vm.ListarNoticiasCommand.Execute(null);
	}
}