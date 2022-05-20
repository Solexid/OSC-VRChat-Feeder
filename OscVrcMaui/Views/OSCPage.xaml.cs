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
    public partial class OSCPage : ContentPage
    {
        private OscViewModel _viewModel;
        public OSCPage()
        {
            InitializeComponent();
            _viewModel = new OscViewModel();
            BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            ItemsListView.ItemsSource = _viewModel.Messages;
        }
    }
}