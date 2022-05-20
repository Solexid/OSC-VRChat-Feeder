using OscVrcMaui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OscVrcMaui.Services
{
 public   class ConfigService
    {
         string configPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            "config.json");
        Config config = null;
        public ConfigService()
        {
            config = LoadConfig(true);
        }

        public Config LoadConfig(bool reload = false)
        {

            if (config == null || reload)
                try
                {
                    var file = File.ReadAllText(configPath);
                    config = file.DeserializeString<Config>();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.SerializeToString());
                    config = new Config();
                    WriteConfig(config);


                }
            return config;

        }




        public bool WriteConfig(Config conf)
        {

           
            try
            {
                File.WriteAllText(configPath, conf.SerializeToString());
                config = conf;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.SerializeToString());
                return false;
            }

            return true;

        }

    }
}
