using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DatabaseLayer
{
    public class AppConfig
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(path, false);
                var root = configurationBuilder.Build();
                return root.GetSection("ConnectionStrings").GetSection("ConnectionString").Value;


            }
        }
    }
}
