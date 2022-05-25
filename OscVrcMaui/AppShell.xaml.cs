using OscVrcMaui.ViewModels;
using OscVrcMaui.Views;
using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace OscVrcMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
          
            Routing.RegisterRoute(nameof(DeviceSelectPage), typeof(DeviceSelectPage));
            Routing.RegisterRoute(nameof(NewProfilePage), typeof(NewProfilePage));
        }

    }
}
