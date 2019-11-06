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
            // TODO: Fix this migration to not be dynamic.
            connection.CreateTable<Account>();
            connection.CreateTable<Transaction>();
            connection.CreateTable<PendingPayPalOrder>();
        }

        public int Version => Versions.InitialTypes;
    }
}