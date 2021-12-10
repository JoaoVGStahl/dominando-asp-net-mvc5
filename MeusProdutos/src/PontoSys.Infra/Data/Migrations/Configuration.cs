using PontoSys.Infra.Data.Context;
using System.Data.Entity.Migrations;
namespace PontoSys.Infra.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppMvcContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
