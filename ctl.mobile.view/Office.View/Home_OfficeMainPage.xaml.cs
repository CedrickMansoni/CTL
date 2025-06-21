using ctl.mobile.viewmodel.Share.ViewModel;

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
		var vm = (Noticia_ViewModel)BindingContext;
		vm.ListarNoticiasCommand.Execute(null);
	}
}