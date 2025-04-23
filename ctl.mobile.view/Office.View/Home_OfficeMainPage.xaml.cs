using ctl.mobile.viewmodel.Office.ViewModel;

namespace ctl.mobile.view.Office.View;

public partial class Home_OfficeMainPage : ContentPage
{
	public Home_OfficeMainPage()
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