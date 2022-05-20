using OscVrcMaui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace OscVrcMaui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewProfilePage : ContentPage
    {
        NewProfileViewModel _viewModel;
        public NewProfilePage()
        {
            InitializeComponent();
            BindingContext = _viewModel  = new NewProfileViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}