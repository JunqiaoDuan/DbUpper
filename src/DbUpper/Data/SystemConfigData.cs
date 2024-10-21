using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbUpper.Data
{
    public class SystemConfigData
    {

        public const string CONFIG_NAME = "SystemConfig";

        #pragma warning disable CS8618

        public bool CreateDbIfNotExist { get; set; }
        public string DbConnStr { get; set; }
    }
}
