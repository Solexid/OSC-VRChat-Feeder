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
    public partial class ProfilesPage : ContentPage
    {
        private ProfilesViewModel _viewModel;

        public ProfilesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ProfilesViewModel();
            _viewModel._page = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void ActiveChanged(CheckBox sender, CheckedChangedEventArgs e)
        {
           
        }
    }
}