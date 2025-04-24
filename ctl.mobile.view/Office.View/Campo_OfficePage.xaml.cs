using ctl.mobile.viewmodel.Office.ViewModel;

namespace ctl.mobile.view.Office.View;

public partial class Campo_OfficePage : ContentPage
{
	public Campo_OfficePage()
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