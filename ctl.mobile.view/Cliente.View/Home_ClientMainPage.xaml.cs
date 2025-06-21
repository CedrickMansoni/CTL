using ctl.mobile.viewmodel.Share.ViewModel;

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
		var vm = (Noticia_ViewModel)BindingContext;
		vm.ListarNoticiasCommand.Execute(null);
	}
}