using OscVrcMaui.Models;
using OscVrcMaui.Services;
using OscVrcMaui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;using Microsoft.Maui.Controls;

namespace OscVrcMaui.ViewModels
{
   public class ProfilesViewModel:BaseViewModel
    {
        private Profile _selectedItem;
        public Page _page { get; set; }
        public ObservableCollection<Profile> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Profile> ItemTapped { get; }
        public Command<Profile> ItemDoubleTapped { get; }
        public Command<Profile> ItemDelete { get; }

        public ProfilesViewModel()
        {
            Title = "OSC Profiles";
            Items = new ObservableCollection<Profile>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemDoubleTapped= new Command<Profile>(OnItemSelected);
            ItemTapped = new Command<Profile>(OnItemClicked);
            ItemDelete =new Command<Profile>(OnDelete);
            AddItemCommand = new Command(OnAddItem);
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await ProfileStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Profile SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewProfilePage));
        }

        async void OnItemSelected(Profile item)
        {
            if (item == null)
                return;
            string action = await _page.DisplayActionSheet("Remove or edit profile?", "Cancel","Edit", "Remove");
            if (action == "Remove")
            {
                await ProfileStore.DeleteItemAsync(item.Id);

            }
            else if (action == "Edit")
            {
                await Shell.Current.GoToAsync($"{nameof(NewProfilePage)}?{nameof(NewProfileViewModel.ProfileId)}={item.Id}");
            }
        }

        async void OnItemClicked(Profile item)
        {
            if (item == null)
                return;
            item.isActive = !item.isActive;
          await  ProfileStore.UpdateItemAsync(item);
            Items[Items.IndexOf(item)] = item;

           
          DependencyService.Get<DeviceSensorsService>().SetRotationSensorStatus(await ProfileStore.HasActiveInputs(InputType.DeviceRotationX, InputType.DeviceRotationY, InputType.DeviceRotationZ));

        }
        async void OnDelete(Profile item)
        {
            if (item == null)
                return;
            string action = await _page.DisplayActionSheet("Remove profile?", "Cancel", "Delete");
            if (action == "Delete") {
                await ProfileStore.DeleteItemAsync(item.Id);

            }
    
          


            
        }
    }
}
