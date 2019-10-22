using System.Threading.Tasks;

namespace PlumPack.Wallet.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PlumPack.Web.Main.Program.Main<Startup>(5002, "/etc/plumpack/wallet", args, host => Task.CompletedTask);
        }
    }
}
