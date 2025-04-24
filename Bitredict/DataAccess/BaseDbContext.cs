using Bitredict.Models;
using Microsoft.EntityFrameworkCore;

namespace Bitredict.DataAccess;

public class BaseDbContext : DbContext
{
    public DbSet<Match> Matchs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<HomePageStatistics> HomePageStatistics { get; set; }
    public DbSet<MatchCenterStatistics> MatchCenterStatistics { get; set; }
    public DbSet<Accuracy> Accuracies { get; set; }
    public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
    {
    }

    public BaseDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server=tcp:bitredict2db.database.windows.net,1433;Initial Catalog=Bitredict2Db;Persist Security Info=False;User ID=tugrulhan;Password=h2002T2000.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        // optionsBuilder.UseSqlServer("Server=bitredict.clgu4comsi0c.eu-north-1.rds.amazonaws.com;User=admin;Password=20002002;");
        optionsBuilder.UseSqlServer(
     "Server=bitredict.clgu4comsi0c.eu-north-1.rds.amazonaws.com;" +
      "Database=Bitredict;" +
     "User=admin;" +
     "Password=20002002;" +
     "TrustServerCertificate=true;"
 );

        //  optionsBuilder.UseSqlServer("Server=tcp:bitredictdbserver.database.windows.net,1433;Initial Catalog=Bitredict_db;Persist Security Info=False;User ID=turgutkarslı;Password=h2002T2000.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    }
}
