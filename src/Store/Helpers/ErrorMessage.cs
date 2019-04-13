using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Helpers
{
    public class ErrorMessage
    {
        public string ReturnErrorMessage(string section, string message)
        {
            var configuration = new ConfigurationManager();
            var tempSection = configuration.Configuration.GetSection(section);

            return tempSection[message];
        }
    }
}
