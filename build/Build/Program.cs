using Build.Buildary;
using Build.Common;
using static Bullseye.Targets;

namespace Build
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = Runner.ParseOptions<Runner.RunnerOptions>(args);
            
            ProjectDefinition.Register(options, new ProjectDefinition
            {
                ProjectName = "Wallet",
                SolutionPath = "./PlumPack.Wallet.sln",
                WebProjectPath = "./src/PlumPack.Wallet.Web/PlumPack.Wallet.Web.csproj",
                DockerImageName = "plumpack/wallet"
            });
            
            Runner.Execute(options);
        }
    }
}
