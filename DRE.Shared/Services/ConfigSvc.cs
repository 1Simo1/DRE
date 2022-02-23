using DRE.Interfaces;
using DRE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Uno.Extensions.Serialization;

namespace DRE.Services
{
    public class ConfigSvc
    {
        public Config config { get; set; }

        public ConfigSvc()
        {
            String jsonConfigFile = $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json";

#if DEBUG
            jsonConfigFile = $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.development.json";
#endif

            if (File.Exists(jsonConfigFile))
            {

                try
                {
                    config = (Config) new SystemTextJsonStreamSerializer()
                        .FromString(File.ReadAllText(jsonConfigFile), typeof(Config));
                }
                catch (Exception ex) { }

            }
        }
    
        
    
    }
}
