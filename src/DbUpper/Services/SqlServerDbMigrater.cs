using DbUp.Engine;
using DbUp;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUpper.Interfaces;
using DbUpper.Models;

namespace DbUpper.Services
{
    public class SqlServerDbMigrater : IDbMigrater
    {

        private DbMigraterContext _dbMigraterContext;
        private UpgradeEngine? _upgradeEngine;

        public SqlServerDbMigrater(DbMigraterContext dbMigraterContext)
        {
            _dbMigraterContext = dbMigraterContext;
        }

        public bool IsDbExist()
        {
            var _sqlConnBuilder = new SqlConnectionStringBuilder(_dbMigraterContext.ConnStr);
            var _originalDbName = _sqlConnBuilder.InitialCatalog;

            // change to master
            _sqlConnBuilder.InitialCatalog = "master";
            var _newConnStr = _sqlConnBuilder.ConnectionString;

            using (var sqlConnection = new SqlConnection(_newConnStr))
            {
                sqlConnection.Open();
                var command = new SqlCommand($"SELECT database_id FROM sys.databases WHERE Name = '{_originalDbName}'", sqlConnection);
                return command.ExecuteScalar() != null;
            }
        }

        public void CreateDb()
        {
            var _sqlConnBuilder = new SqlConnectionStringBuilder(_dbMigraterContext.ConnStr);
            var _originalDbName = _sqlConnBuilder.InitialCatalog;

            // change to master
            _sqlConnBuilder.InitialCatalog = "master";
            var _newConnStr = _sqlConnBuilder.ConnectionString;

            using (var sqlConnection = new SqlConnection(_newConnStr))
            {
                sqlConnection.Open();
                var command = new SqlCommand($"CREATE DATABASE [{_originalDbName}]", sqlConnection);
                command.ExecuteNonQuery();
            }
        }

        public void BuildEngine(Func<UpgradeEngine>? customEngine)
        {
            if (customEngine != null)
            {
                return;
            }

            _upgradeEngine = DeployChanges
                .To
                .SqlDatabase(_dbMigraterContext.ConnStr)
                .WithScriptsFromFileSystem(_dbMigraterContext.ScriptPath, s => s.StartsWith(_dbMigraterContext.ScriptPath))
                .LogToConsole()
                .Build();
        }

        public List<SqlScript> GetScriptsToExecute()
        {
            if (_upgradeEngine == null)
            {
                throw new Exception("Please execute [BuildEngine] first. ");
            }

            var waitingExecutedScripts = _upgradeEngine
                .GetScriptsToExecute();
            return waitingExecutedScripts ?? new List<SqlScript>();
        }

        public DatabaseUpgradeResult MigrateDb()
        {
            if (_upgradeEngine == null)
            {
                throw new Exception("Please execute [BuildEngine] first. ");
            }

            var result = _upgradeEngine.PerformUpgrade();
            return result;
        }

    }
}
