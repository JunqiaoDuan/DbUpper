using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbUpper.Models
{
    public class DbMigraterContext
    {
        public DbMigraterContext(string connStr, string scriptPath)
        {
            ConnStr = connStr;
            ScriptPath = scriptPath;
        }

        public string ConnStr { get; set; }
        public string ScriptPath { get; set; }

    }
}
