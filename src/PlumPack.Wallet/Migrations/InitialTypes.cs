using System.Data;
using PlumPack.Wallet.Domain;
using ServiceStack.OrmLite;
using SharpDataAccess.Migrations;
namespace PlumPack.Wallet.Migrations
{
    [Migration]
    public class InitialTypes : IMigration
    {
        public void Run(IDbConnection connection)
        {
            connection.ExecuteSql(@"
CREATE TABLE ""accounts""
(
    ""id"" TEXT PRIMARY KEY 
); 
");
            connection.ExecuteSql(@"
CREATE TABLE ""transactions"" 
(
""id"" TEXT PRIMARY KEY, 
""amount"" DECIMAL(38,6) NOT NULL 
); 
"); 
        }

        public int Version => Versions.InitialTypes;
    }
}