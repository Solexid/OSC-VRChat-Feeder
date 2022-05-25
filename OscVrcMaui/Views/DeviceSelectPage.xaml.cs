using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using OscVrcMaui.ViewModels;

namespace OscVrcMaui.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceSelectPage : ContentPage
    {
        DeviceSelectViewModel _viewModel;
        public DeviceSelectPage ()
		{
            _viewModel=new DeviceSelectViewModel();
            this.BindingContext = _viewModel;
            InitializeComponent ();

		}
        protected override void OnAppearing()
        {
          _viewModel.OnAppearing(); 
        }
    }
}