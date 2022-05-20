using OscVrcMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;using Microsoft.Maui.Controls;

namespace OscVrcMaui.Services
{
  public  class ProfileDataStore :IDataStore<Profile>
    {
        private List<Profile> items;
        public ConfigService configService => DependencyService.Get<ConfigService>();
        public ProfileDataStore()
        {
            items = configService.LoadConfig(true).Profiles;
        }
        public async Task<bool> AddItemAsync(Profile item)
        {
            items.Add(item);
           await UpdateConfig();
            return await Task.FromResult(true);
        }
        private async Task UpdateConfig() {

            await Task.Run(()=> {

            var config= configService.LoadConfig();
                config.Profiles = items;
                configService.WriteConfig(config);
            
            });
        
        
        }



        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where(arg=> arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);
            await UpdateConfig();
            return await Task.FromResult(true);
        }

        public async Task<Profile> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }
        public async Task<bool> HasActiveInputs(params InputType[] input)
        {
            
            return await Task.Run(()=>{ 


                bool result = false;
                foreach (InputType item in input)
                {
                    if (items.Any(c => c.Input == item && c.isActive))
                        result = true;
                }
                return result;
            
            });
        }

        public async Task<IEnumerable<Profile>> GetItemsByTypeAsync(InputType input, bool forceRefresh = false)
        {
            if (forceRefresh)
                items = configService.LoadConfig(true).Profiles;
            return await Task.FromResult(items.Where(c=>c.isActive&&c.Input== input));
        }
        public async Task<IEnumerable<Profile>> GetItemsAsync(bool forceRefresh = false)
        {
            if(forceRefresh)
                items = configService.LoadConfig(true).Profiles;
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateItemAsync(Profile item)
        {
            var oldItem = items.Where(arg=> arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            await UpdateConfig();
            return await Task.FromResult(true);
        }
    }
}
