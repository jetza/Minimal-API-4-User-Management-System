global using Microsoft.EntityFrameworkCore;

namespace MinimalAPI
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //konstruktor
        }
        public DbSet<User> Users => Set<User>(); //dodavanje tabela
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UsersPermissions> UsersPermissions => Set<UsersPermissions>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)//uvezivanje tabela permission i user preko userPermissions
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UsersPermissions>(entity =>
            {
                entity.HasMany<Permission>();
                entity.HasMany<User>();
            });

            modelBuilder.Entity<User>(entity => {
                entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();
            });
        }
    }
}
