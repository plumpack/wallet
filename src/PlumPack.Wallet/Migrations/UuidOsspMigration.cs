using System.Data;
using ServiceStack.OrmLite;
using SharpDataAccess.Migrations;

namespace PlumPack.Wallet.Migrations
{
    [Migration]
    public class UuidOsspMigration : IMigration
    {
        public void Run(IDbConnection connection)
        {
            connection.ExecuteSql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");
        }

        public int Version => Versions.UuidOssp;
    }
}