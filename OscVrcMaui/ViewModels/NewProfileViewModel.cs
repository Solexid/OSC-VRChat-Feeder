using OscVrcMaui.Converters;
using OscVrcMaui.Models;
using OscVrcMaui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;using Microsoft.Maui.Controls;

namespace OscVrcMaui.ViewModels
{
    [QueryProperty(nameof(ProfileId), nameof(ProfileId))]
    public class NewProfileViewModel : BaseViewModel
    {
        private bool ValidateSave()
        {
            return true;
        }
        public List<string> InputTypes
        {
            get
            {
                return Enum.GetNames(typeof(InputType)).Select(b => b.SplitCamelCase()).ToList();
            }
        }
        public string ProfileId { set
            {
                Load(value);
            } }
        public string id { get; set; }
        public String profileName;
        public InputType input;
        public String rootPath = "/avatar/parameters/";
        public String parameterName = "NewParam";
        public bool normalize = true;
        public int maxValue = 175;
        public int minValue = 55;
        public InputType Input
        {
            get => input;
            set => SetProperty(ref input, value);
        }
        public string ProfileName
        {
            get => profileName;
            set => SetProperty(ref profileName, value);
        }
        public string RootPath
        {
            get => rootPath;
            set => SetProperty(ref rootPath, value);
        }
        public string ParameterName
        {
            get => parameterName;
            set => SetProperty(ref parameterName, value);
        }
        public bool Normalize
        {
            get => normalize;
            set => SetProperty(ref normalize, value);
        }
        public string MaxValue
        {
            get { return maxValue.ToString(); }
            set { if (value != "0" && value != "" && value != null) SetProperty(ref maxValue, Int32.Parse(value)); }
        }

        public string MinValue
        {
            get { return minValue.ToString(); }
            set { if (value != "0" && value != "" && value != null) SetProperty(ref minValue, Int32.Parse(value)); }
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public NewProfileViewModel()
        {


            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
        }
        public async void OnAppearing() {

          
        
        }
        private async Task Load(string value) {

            id = value;
            var profile = await ProfileStore.GetItemAsync(value);
            ProfileName = profile.ProfileName;
            ParameterName = profile.ParameterName;
            RootPath = profile.RootPath;
            Input = profile.Input;
            Normalize = profile.Normalize;
            MaxValue = profile.MaxValue.ToString();
            MinValue = profile.MinValue.ToString();
        }
        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Profile newItem = new Profile()
            {
                Id =id!=null?id:Guid.NewGuid().ToString(),
                ProfileName = profileName,
                ParameterName = parameterName,
                Input = Input,
                isActive = true,
                LastValue = "",
                RootPath = rootPath,
                MaxValue = maxValue,
                MinValue = minValue,
                Normalize = normalize,
                CreationDate = DateTime.Now
            };
           
            await ProfileStore.UpdateItemAsync(newItem);
           
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }




    }
}
