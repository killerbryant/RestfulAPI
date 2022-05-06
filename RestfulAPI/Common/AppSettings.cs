using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPI.Utils
{
    public class AppSettings
    {
        private static readonly object objLock = new object();
        private static AppSettings instance = null;

        private IConfigurationRoot Config { get; }

        private AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Config = builder.Build();
        }

        public static AppSettings GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new AppSettings();
                    }
                }
            }

            return instance;
        }

        public static string GetConfigure(string name)
        {
            return GetInstance().Config.GetSection(name).Value;
        }
    }
}
