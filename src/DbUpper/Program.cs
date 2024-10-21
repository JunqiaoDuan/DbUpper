using DbUpper.Data;
using DbUpper.Enums;
using DbUpper.Interfaces;
using DbUpper.Models;
using DbUpper.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

#region Read Config File

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<SystemConfigData>(context.Configuration.GetRequiredSection(SystemConfigData.CONFIG_NAME));
    })
    .Build();
var configData = host.Services.GetRequiredService<IOptions<SystemConfigData>>().Value;

#endregion

#region Init

var consoleLogger = new ConsoleLogger();
consoleLogger.WriteLine();

var connStr = configData?.DbConnStr;
if (connStr == null)
{
    consoleLogger.WriteLine(LogType.Error, "DbConnStr is null. ");
    return;
}
var dbMigraterContext = new DbMigraterContext(connStr, @".\Scripts");
IDbMigrater dbMigrater = new SqlServerDbMigrater(dbMigraterContext);

#endregion

#region Execute Db Migrater

if (configData?.CreateDbIfNotExist == true
    && !dbMigrater.IsDbExist())
{
    dbMigrater.CreateDb();

    consoleLogger.WriteLine(LogType.Debug, $"Database has been created.");
}

dbMigrater.BuildEngine();
var waitingExecutedScripts = dbMigrater.GetScriptsToExecute();
if (waitingExecutedScripts.Count == 0)
{
    consoleLogger.WriteLine(LogType.Info, "======================== Don't Need to Update ========================");

    return;
}

consoleLogger.WriteLine();
consoleLogger.WriteLine();
consoleLogger.WriteLine(LogType.Info, "======================== Need Update This Time: ========================");
foreach (var item in waitingExecutedScripts)
{
    consoleLogger.WriteLine(LogType.Info, "--------Script Name--------");
    consoleLogger.WriteLine(LogType.Info, item.Name);
    consoleLogger.WriteLine(LogType.Info, "--------Script Content--------");
    consoleLogger.WriteLine(LogType.Info, item.Contents);
}

consoleLogger.WriteLine(LogType.Info, "Are you sure to update？(Please backup your database first)[y/n]");
var inputKey = Console.Read();
consoleLogger.WriteLine(LogType.Info, "answer:");
consoleLogger.WriteLine(LogType.Info, inputKey.ToString());
if (inputKey != 121 && inputKey != 89)          // y
{
    consoleLogger.WriteLine(LogType.Warn, "========================You have refused the update ========================");

    return;
}

var result = dbMigrater.MigrateDb();
if (!result.Successful)
{
    consoleLogger.WriteLine(LogType.Error, "", result.Error);
}
else
{
    consoleLogger.WriteLine(LogType.Info, "======================== Successfully ========================");
}

#endregion
