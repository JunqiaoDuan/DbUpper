using DbUp.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbUpper.Interfaces
{
    public interface IDbMigrater
    {
        public bool IsDbExist();

        public void CreateDb();

        public void BuildEngine(Func<UpgradeEngine>? customEngine = null);

        public List<SqlScript> GetScriptsToExecute();

        public DatabaseUpgradeResult MigrateDb();
    }
}
