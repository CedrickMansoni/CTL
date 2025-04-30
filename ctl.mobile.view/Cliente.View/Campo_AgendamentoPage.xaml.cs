using ctl.mobile.viewmodel.Client.ViewModel;

namespace ctl.mobile.view.Cliente.View;

public partial class Campo_AgendamentoPage : ContentPage
{
	public Campo_AgendamentoPage()
	{
		InitializeComponent();

		var vm = (Campo_AgendamentoViewModel)BindingContext;

		vm.ListarMarcacoesCommand.Execute(null);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		
		try
		{
			var vm = (Campo_AgendamentoViewModel)BindingContext;
			vm.ListarMarcacoesCommand.Execute(null);
		}
		catch 
		{

		}
		
    }
}