using ctl.mobile.viewmodel.Office.ViewModel;

namespace ctl.mobile.view.Cliente.View;

public partial class Campo_ClientPage : ContentPage
{
	public Campo_ClientPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var vm = (Campo_MainPageViewModel)BindingContext;
		vm.ListarCampoCommand.Execute(null);
    }
}